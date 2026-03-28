using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NarutoGame.Application.Mappings;
using NarutoGame.Application.Services;
using NarutoGame.Core.Interfaces;
using NarutoGame.Infrastructure.Data;
using NarutoGame.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings__DefaultConnection não está configurado. Defina a variável de ambiente ou configuração.");
}

builder.Services.AddDbContext<NarutoGameDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

// Configure Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<INpcRepository, NpcRepository>();
builder.Services.AddScoped<IMissionRepository, MissionRepository>();

// Configure Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<GameService>();

    // Configure Rate Limiting
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        
        // ============================================================
        // HELPER FUNCTIONS FOR RATE LIMITING
        // ============================================================
        
        /// <summary>
        /// Obtém o IP do cliente considerando proxies e balanceadores de carga.
        /// Verifica os headers X-Forwarded-For e X-Real-IP antes de usar RemoteIpAddress.
        /// </summary>
        string GetClientIp(HttpContext context)
        {
            // Check X-Forwarded-For header first (for load balancers/proxies)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                // X-Forwarded-For can contain multiple IPs, take the first one (original client)
                var firstIp = forwardedFor.Split(',')[0].Trim();
                if (!string.IsNullOrEmpty(firstIp))
                    return firstIp;
            }
            
            // Check X-Real-IP header (common with nginx)
            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
                return realIp;
            
            // Fall back to remote IP address
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        /// <summary>
        /// Obtém a partition key híbrida para rate limiting.
        /// - Para usuários autenticados: usa UserId (previne abuse entre múltiplos IPs do mesmo usuário)
        /// - Para usuários anônimos: usa IP (previne abuse de uma mesma origem)
        /// </summary>
        string GetHybridPartitionKey(HttpContext context)
        {
            // Se o usuário estiver autenticado, usar UserId como partition key
            // Isso garante que cada usuário autenticado tenha sua própria quota,
            // independentemente do IP de onde esteja acessando
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                          ?? context.User.FindFirst("sub")?.Value
                          ?? context.User.FindFirst("userId")?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    return $"user:{userId}";
                }
            }
            
            // Para usuários não autenticados, usar IP
            return $"ip:{GetClientIp(context)}";
        }
        
        // ============================================================
        // RATE LIMITING POLICIES
        // ============================================================

        // Auth endpoints: strict rate limiting by IP only to prevent brute force
        // IMPORTANTE: Não usa UserId aqui pois usuários mal-intencionados podem
        // tentar autenticar com diferentes contas a partir do mesmo IP
        options.AddPolicy("AuthPolicy", context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: GetClientIp(context),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(15),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                }));

        // Standard API: moderate rate limiting with hybrid key (IP + UserId)
        // Para usuários autenticados: limita por UserId (cada usuário tem sua quota)
        // Para usuários anônimos: limita por IP (cada origem tem sua quota)
        options.AddPolicy("StandardPolicy", context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: GetHybridPartitionKey(context),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 10
                }));

        // AuthenticatedPolicy: rate limiting estrito para usuários autenticados
        // Garante que usuários autenticados não façam abuse da API
        // Este policy requer que o usuário esteja autenticado (retorna 401 se não estiver)
        options.AddPolicy("AuthenticatedPolicy", context =>
        {
            var partitionKey = GetHybridPartitionKey(context);
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: partitionKey,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 200,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 20
                });
        });
    });

// Configure JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] 
    ?? throw new InvalidOperationException("Jwt:Secret não está configurado. Defina a variável de ambiente ou configuração.");

// Validate JWT secret length (minimum 32 characters for security)
if (jwtSecret.Length < 32)
{
    throw new InvalidOperationException("Jwt:Secret deve ter pelo menos 32 caracteres para garantir segurança adequada.");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "NarutoGameAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "NarutoGame";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddAuthorization();

// Configure CORS
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins")
    .Get<string[]>() ?? new[] { "http://localhost:5173", "http://localhost:5174", "http://127.0.0.1:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
              .WithHeaders("Authorization", "Content-Type", "Accept")
              .AllowCredentials();
    });
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Naruto Game API",
        Version = "v1",
        Description = "API para o jogo Naruto - Vila da Folha"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseCors("AllowFrontend");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("Naruto Game API starting up...");

app.Run();
