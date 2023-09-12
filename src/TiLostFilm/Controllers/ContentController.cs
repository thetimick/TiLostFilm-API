using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Content;
using TiLostFilm.Entities.Error;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ContentController: ControllerBase
{
    private readonly ILogger<ContentController> _logger;
    private readonly ContentService _contentService;

    public ContentController(ILogger<ContentController> logger, ContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Serials")]
    public async Task<ActionResult<ContentEntity>> GetSerials(
        int offset = 0, 
        int? genre = null,
        int? year = null,
        int? channel = null,
        int? type = null,
        int? country = null
    ) {
        try
        {
            return await _contentService.ObtainSerials(offset, genre, year, channel, type, country);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Movies")]
    public async Task<ActionResult<ContentEntity>> GetMovies(
        int offset = 0, 
        int? genre = null,
        int? year = null,
        int? type = null,
        int? country = null
    ) {
        try
        {
            return await _contentService.ObtainMovies(offset, genre, year, type, country);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Filters")]
    public async Task<ActionResult<ContentFiltersEntity>> GetFilters([Required] ContentType type)
    {
        try
        {
            return await _contentService.ObtainFilters(type);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}