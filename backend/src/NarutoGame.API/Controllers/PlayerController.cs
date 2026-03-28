using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NarutoGame.Application.DTOs;
using NarutoGame.Application.Services;

namespace NarutoGame.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("StandardPolicy")]
public class PlayerController : BaseApiController
{
    private readonly PlayerService _playerService;

    public PlayerController(PlayerService playerService)
    {
        _playerService = playerService;
    }

    /// <summary>
    /// Obtém o perfil do jogador autenticado
    /// </summary>
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var unauthorized = UnauthorizedIfNotAuthenticated();
        if (unauthorized != null) return unauthorized;

        var player = await _playerService.GetPlayerByUserIdAsync(GetAuthenticatedUserId()!.Value);
        
        if (player == null)
        {
            return NotFound(new { error = "Jogador não encontrado." });
        }

        return Ok(player);
    }

    /// <summary>
    /// Atualiza o perfil do jogador
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdatePlayerRequest request)
    {
        var unauthorized = UnauthorizedIfNotAuthenticated();
        if (unauthorized != null) return unauthorized;

        var (success, player, error) = await _playerService.UpdatePlayerAsync(GetAuthenticatedUserId()!.Value, request);

        if (!success)
        {
            return BadRequest(new { error });
        }

        return Ok(player);
    }

    /// <summary>
    /// Adiciona XP ao jogador
    /// </summary>
    [HttpPost("xp")]
    public async Task<IActionResult> AddXp([FromBody] AddXpRequest request)
    {
        var unauthorized = UnauthorizedIfNotAuthenticated();
        if (unauthorized != null) return unauthorized;

        var response = await _playerService.AddXpAsync(GetAuthenticatedUserId()!.Value, request.Amount);

        if (!response.Success)
        {
            return BadRequest(new { error = response.Message });
        }

        return Ok(response);
    }

    /// <summary>
    /// Atualiza as habilidades do jogador
    /// </summary>
    [HttpPut("skills")]
    public async Task<IActionResult> UpdateSkills([FromBody] UpdateSkillsRequest request)
    {
        var unauthorized = UnauthorizedIfNotAuthenticated();
        if (unauthorized != null) return unauthorized;

        var (success, error) = await _playerService.UpdateSkillsAsync(GetAuthenticatedUserId()!.Value, request);

        if (!success)
        {
            return BadRequest(new { error });
        }

        return Ok(new { message = "Habilidades atualizadas com sucesso!" });
    }
}
