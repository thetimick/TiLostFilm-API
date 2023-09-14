using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Auth;
using TiLostFilm.Auth.Entities;

namespace TiLostFilm.Controllers;

/// <inheritdoc />
[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly AuthService _authService;

    /// <inheritdoc />
    public AuthController(ILogger<AuthController> logger, AuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<ActionResult<AccessTokenEntity>> Login(LoginRequestEntity entity)
    {
        _logger.LogInformation("Login ({Login}|{Password})", entity.Login, entity.Password);
        
        await _authService.Login(entity);
        
        return new ActionResult<AccessTokenEntity>(new AccessTokenEntity());
    }
}