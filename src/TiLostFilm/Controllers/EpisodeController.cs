using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Episode;
using TiLostFilm.Entities.Error;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class EpisodeController: ControllerBase
{
    private readonly ILogger<EpisodeController> _logger;
    private readonly EpisodeService _episodeService;

    public EpisodeController(ILogger<EpisodeController> logger, EpisodeService episodeService)
    {
        _logger = logger;
        _episodeService = episodeService;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet]
    public async Task<ActionResult<EpisodeEntity>> GetEpisode([Required] string? url)
    {
        try
        {
            return url is null 
                ? new UnprocessableEntityResult() 
                : new ActionResult<EpisodeEntity>(await _episodeService.GetEpisode(url));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}