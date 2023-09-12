using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Error;
using TiLostFilm.Entities.Shedule;
using TiLostFirm.Parser;

// ReSharper disable UnusedMember.Global

namespace TiLostFilm.Controllers;

/// <summary>
/// Расписание
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class SheduleController: ControllerBase
{
    private readonly ILogger<SheduleController> _logger;
    private readonly SheduleService _sheduleService;


    /// <inheritdoc />
    public SheduleController(ILogger<SheduleController> logger, SheduleService sheduleService)
    {
        _logger = logger;
        _sheduleService = sheduleService;
    }
    
    /// <summary>
    /// Расписание
    /// </summary>
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