using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Episode;
using TiLostFilm.Entities.Error;
using TiLostFilm.Entities.Shedule;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class SheduleController
{
    private readonly ILogger<SheduleController> _logger;
    private readonly SheduleService _sheduleService;

    public SheduleController(ILogger<SheduleController> logger, SheduleService sheduleService)
    {
        _logger = logger;
        _sheduleService = sheduleService;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet]
    public async Task<ActionResult<SheduleEntity>> GetShedule()
    {
        try
        {
            return new ActionResult<SheduleEntity>(await _sheduleService.GetShedule());
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}