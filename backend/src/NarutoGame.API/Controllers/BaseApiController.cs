using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace NarutoGame.API.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Extrai o ID do usuário autenticado a partir dos claims do JWT.
    /// </summary>
    /// <returns>O Guid do usuário ou null se não autenticado.</returns>
    protected Guid? GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    /// <summary>
    /// Retorna Unauthorized se o usuário não estiver autenticado.
    /// </summary>
    protected IActionResult? UnauthorizedIfNotAuthenticated()
    {
        var userId = GetAuthenticatedUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = "Usuário não autenticado." });
        }
        return null;
    }
}
