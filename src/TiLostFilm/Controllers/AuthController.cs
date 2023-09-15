using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Auth;
using TiLostFilm.Auth.Entities;
using TiLostFilm.Entities.Error;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorEntity))]
    [HttpPost("Login")]
    public async Task<ActionResult<AccessTokenEntity>> Login(LoginRequestEntity entity)
    {
        try
        {
            return new ActionResult<AccessTokenEntity>(await _authService.LoginAsync(entity));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError("UnauthorizedAccessException: {ExMessage}", ex.Message);
            return new UnauthorizedObjectResult(new ErrorEntity(StatusCodes.Status401Unauthorized, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}