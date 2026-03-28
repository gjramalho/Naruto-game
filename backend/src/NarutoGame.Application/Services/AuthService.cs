using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NarutoGame.Application.DTOs;
using NarutoGame.Core.Entities;
using NarutoGame.Core.Interfaces;

namespace NarutoGame.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly int _bcryptWorkFactor;

    public AuthService(
        IUserRepository userRepository,
        IPlayerRepository playerRepository,
        IMapper mapper,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _mapper = mapper;
        _configuration = configuration;
        
        // Safely parse BCrypt work factor with fallback to default (12)
        if (!int.TryParse(configuration["Password:BCryptWorkFactor"], out int workFactor))
            workFactor = 12;
        _bcryptWorkFactor = workFactor;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
        {
            return (false, "E-mail inválido.");
        }

        // Validate password
        if (!IsValidPassword(request.Password))
        {
            return (false, "A senha deve ter pelo menos 8 caracteres, incluindo maiúsculas, minúsculas, números e um caractere especial.");
        }

        // Validate nickname
        if (string.IsNullOrWhiteSpace(request.Nickname) || request.Nickname.Length < 3 || request.Nickname.Length > 50)
        {
            return (false, "O nickname deve ter entre 3 e 50 caracteres.");
        }

        // Check if email already exists
        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            return (false, "E-mail já cadastrado.");
        }

        // Check if nickname already exists
        if (await _userRepository.ExistsByNicknameAsync(request.Nickname))
        {
            return (false, "Nome ninja já está em uso.");
        }

        // Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLower().Trim(),
            Nickname = request.Nickname.Trim(),
            PasswordHash = HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Create player for the user
        var player = new Player
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = request.CharacterName ?? request.Nickname,
            Level = 1,
            Xp = 0,
            MaxXp = 100,
            Chakra = 100,
            CreatedAt = DateTime.UtcNow,
            Stats = new PlayerStats
            {
                Id = Guid.NewGuid(),
                PlayerId = Guid.Empty,
                Ninjutsu = 1,
                Taijutsu = 1,
                Genjutsu = 1
            }
        };

        // Set the PlayerId reference in Stats after player creation
        player.Stats.PlayerId = player.Id;

        // Use transaction to ensure both user and player are created together
        var transaction = await _userRepository.BeginTransactionAsync();
        try
        {
            await _userRepository.AddAsync(user);
            await _playerRepository.AddAsync(player);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }

        return (true, null);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailOrNicknameAsync(request.Identifier);

        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Error = "Conta não encontrada. Crie sua conta primeiro."
            };
        }

        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            return new LoginResponse
            {
                Success = false,
                Error = "Senha incorreta."
            };
        }

        if (!user.IsActive)
        {
            return new LoginResponse
            {
                Success = false,
                Error = "Conta desativada."
            };
        }

        var token = GenerateJwtToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new LoginResponse
        {
            Success = true,
            Token = token,
            ExpiresAt = expiresAt,
            User = _mapper.Map<UserDto>(user)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSecret = _configuration["Jwt:Secret"] 
            ?? throw new InvalidOperationException("Jwt:Secret não está configurado.");
            
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Nickname),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "NarutoGameAPI",
            audience: _configuration["Jwt:Audience"] ?? "NarutoGame",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(_bcryptWorkFactor));
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private static bool IsValidEmail(string email)
    {
        // RFC 5322 compliant email validation using MailAddress
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email.Trim().ToLower();
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPassword(string password)
    {
        // At least 8 chars, 1 upper, 1 lower, 1 number, 1 special char
        if (password.Length < 8) return false;
        if (!password.Any(char.IsUpper)) return false;
        if (!password.Any(char.IsLower)) return false;
        if (!password.Any(char.IsDigit)) return false;
        if (!password.Any(c => !char.IsLetterOrDigit(c))) return false;
        
        return true;
    }
}
