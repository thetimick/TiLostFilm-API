using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Content;
using TiLostFilm.Entities.Error;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("[controller]")]
[Produces("application/json")]
public class ContentController: ControllerBase
{
    private readonly ILogger<ContentController> _logger;
    private readonly LostFilmParser _lostFilmParser;

    public ContentController(ILogger<ContentController> logger, LostFilmParser lostFilmParser)
    {
        _logger = logger;
        _lostFilmParser = lostFilmParser;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Serials")]
    public async Task<ActionResult<ContentSerialsResponse>> GetSerials(int offset = 0)
    {
        try
        {
            var response = await _lostFilmParser.Content.ObtainSerials(offset);
            Guard.Against.Null(response);
            
            return response;
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
            var response = await _lostFilmParser.Content.ObtainMovies(offset);
            Guard.Against.Null(response);
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}