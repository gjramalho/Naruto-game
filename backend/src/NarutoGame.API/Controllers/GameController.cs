using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NarutoGame.Application.DTOs;
using NarutoGame.Application.Services;

namespace NarutoGame.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("StandardPolicy")]
public class GameController : BaseApiController
{
    private readonly GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Lista todos os NPCs
    /// </summary>
    [HttpGet("npcs")]
    public async Task<IActionResult> GetNpcs()
    {
        var npcs = await _gameService.GetAllNpcsAsync();
        return Ok(npcs);
    }

    /// <summary>
    /// Obtém um NPC específico pela chave
    /// </summary>
    [HttpGet("npcs/{key}")]
    public async Task<IActionResult> GetNpcByKey(string key)
    {
        var npc = await _gameService.GetNpcByKeyAsync(key);
        
        if (npc == null)
        {
            return NotFound(new { error = "NPC não encontrado." });
        }

        return Ok(npc);
    }

    /// <summary>
    /// Lista todas as missões
    /// </summary>
    [HttpGet("missions")]
    public async Task<IActionResult> GetMissions()
    {
        var missions = await _gameService.GetAllMissionsAsync();
        return Ok(missions);
    }

    /// <summary>
    /// Lista missões diárias
    /// </summary>
    [HttpGet("missions/daily")]
    public async Task<IActionResult> GetDailyMissions()
    {
        var missions = await _gameService.GetDailyMissionsAsync();
        return Ok(missions);
    }

    /// <summary>
    /// Completa uma missão
    /// </summary>
    [Authorize]
    [HttpPost("missions/{missionId}/complete")]
    public async Task<IActionResult> CompleteMission(Guid missionId)
    {
        var unauthorized = UnauthorizedIfNotAuthenticated();
        if (unauthorized != null) return unauthorized;

        var response = await _gameService.CompleteMissionAsync(GetAuthenticatedUserId()!.Value, missionId);

        if (!response.Success)
        {
            return BadRequest(new { error = response.Message });
        }

        return Ok(response);
    }
}
