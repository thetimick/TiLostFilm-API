using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Content;
using TiLostFilm.Entities.Error;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
class ContentController: ControllerBase
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
    public async Task<ActionResult<ContentSerialsResponse>> GetSerials(int offset = 0)
    {
        try
        {
            var response = await _contentService.ObtainSerials(offset);
            Guard.Against.Null(response.Data);
            return response.Data;
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
    public async Task<ActionResult<ContentMoviesResponse>> GetMovies(int offset = 0)
    {
        try
        {
            var response = await _contentService.ObtainMovies(offset);
            Guard.Against.Null(response.Data);
            return response.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}