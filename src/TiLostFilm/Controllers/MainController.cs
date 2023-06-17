using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Error;
using TiLostFilm.Entities.Main;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class MainController: ControllerBase
{
    private readonly ILogger<MainController> _logger;
    private readonly MainService _mainService;

    public MainController(ILogger<MainController> logger, MainService mainService)
    {
        _logger = logger;
        _mainService = mainService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet]
    public async Task<ActionResult<MainEntity>> GetMain()
    {
        try
        {
            return new ActionResult<MainEntity>(await _mainService.GetMain());
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}