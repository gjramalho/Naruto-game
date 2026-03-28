using Moq;
using Microsoft.Extensions.Configuration;
using NarutoGame.Application.Services;
using NarutoGame.Core.Entities;
using NarutoGame.Core.Interfaces;
using AutoMapper;
using NarutoGame.Application.DTOs;

namespace NarutoGame.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();

        // Setup configuration mock
        _configurationMock.Setup(c => c["Password:BCryptWorkFactor"]).Returns("10");
        _configurationMock.Setup(c => c["Jwt:Secret"]).Returns("this-is-a-secret-key-that-is-at-least-32-characters-long!");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _playerRepositoryMock.Object,
            _mapperMock.Object,
            _configurationMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Test@1234",
            Nickname = "TestUser"
        };

        _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.ExistsByNicknameAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

        // Act
        var (success, error) = await _authService.RegisterAsync(request);

        // Assert
        Assert.True(success);
        Assert.Null(error);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        _playerRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Player>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithInvalidEmail_ReturnsFalse()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "invalid-email",
            Password = "Test@1234",
            Nickname = "TestUser"
        };

        // Act
        var (success, error) = await _authService.RegisterAsync(request);

        // Assert
        Assert.False(success);
        Assert.Equal("E-mail inválido.", error);
    }

    [Theory]
    [InlineData("short", false)]              // Too short
    [InlineData("alllowercase1!", false)]     // No uppercase
    [InlineData("ALLUPPERCASE1!", false)]     // No lowercase
    [InlineData("NoNumbersHere!", false)]     // No numbers
    [InlineData("NoSpecialChar123", false)]   // No special chars
    [InlineData("Valid@Pass1", true)]         // Valid password
    public async Task RegisterAsync_WithVariousPasswords_ReturnsExpectedResult(string password, bool expectedSuccess)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = password,
            Nickname = "TestUser"
        };

        _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.ExistsByNicknameAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.BeginTransactionAsync()).ReturnsAsync(new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>().Object);

        // Act
        var (success, error) = await _authService.RegisterAsync(request);

        // Assert
        Assert.Equal(expectedSuccess, success);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ReturnsError()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "existing@example.com",
            Password = "Test@1234",
            Nickname = "TestUser"
        };

        _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

        // Act
        var (success, error) = await _authService.RegisterAsync(request);

        // Assert
        Assert.False(success);
        Assert.Equal("E-mail já cadastrado.", error);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingNickname_ReturnsError()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Test@1234",
            Nickname = "ExistingUser"
        };

        _userRepositoryMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.ExistsByNicknameAsync(It.IsAny<string>())).ReturnsAsync(true);

        // Act
        var (success, error) = await _authService.RegisterAsync(request);

        // Assert
        Assert.False(success);
        Assert.Equal("Nome ninja já está em uso.", error);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Nickname = "TestUser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@1234"),
            IsActive = true
        };

        _userRepositoryMock.Setup(r => r.GetByEmailOrNicknameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto { Id = user.Id, Email = user.Email, Nickname = user.Nickname });

        var request = new LoginRequest { Identifier = "test@example.com", Password = "Test@1234" };

        // Act
        var response = await _authService.LoginAsync(request);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Token);
        Assert.NotNull(response.User);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsFalse()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Nickname = "TestUser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@1234"),
            IsActive = true
        };

        _userRepositoryMock.Setup(r => r.GetByEmailOrNicknameAsync(It.IsAny<string>())).ReturnsAsync(user);

        var request = new LoginRequest { Identifier = "test@example.com", Password = "WrongPassword" };

        // Act
        var response = await _authService.LoginAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Senha incorreta.", response.Error);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.GetByEmailOrNicknameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var request = new LoginRequest { Identifier = "nonexistent@example.com", Password = "Test@1234" };

        // Act
        var response = await _authService.LoginAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Conta não encontrada. Crie sua conta primeiro.", response.Error);
    }

    [Fact]
    public async Task LoginAsync_WithInactiveUser_ReturnsFalse()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Nickname = "TestUser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@1234"),
            IsActive = false
        };

        _userRepositoryMock.Setup(r => r.GetByEmailOrNicknameAsync(It.IsAny<string>())).ReturnsAsync(user);

        var request = new LoginRequest { Identifier = "test@example.com", Password = "Test@1234" };

        // Act
        var response = await _authService.LoginAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Conta desativada.", response.Error);
    }
}
