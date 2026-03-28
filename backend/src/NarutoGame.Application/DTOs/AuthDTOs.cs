using System.ComponentModel.DataAnnotations;

namespace NarutoGame.Application.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [MaxLength(255, ErrorMessage = "E-mail deve ter no máximo 255 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nickname é obrigatório.")]
    [MinLength(3, ErrorMessage = "Nickname deve ter pelo menos 3 caracteres.")]
    [MaxLength(50, ErrorMessage = "Nickname deve ter no máximo 50 caracteres.")]
    public string Nickname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "Senha deve ter pelo menos 8 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "Nome do personagem deve ter no máximo 100 caracteres.")]
    public string? CharacterName { get; set; }
}

public class LoginRequest
{
    [Required(ErrorMessage = "Identificador (e-mail ou nickname) é obrigatório.")]
    public string Identifier { get; set; } = string.Empty; // email or nickname

    [Required(ErrorMessage = "Senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public UserDto? User { get; set; }
    public string? Error { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
}
