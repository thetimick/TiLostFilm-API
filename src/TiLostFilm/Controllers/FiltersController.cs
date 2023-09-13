using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Content;
using TiLostFilm.Entities.Error;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

/// <inheritdoc />
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class FiltersController: ControllerBase
{
    private readonly ILogger<FiltersController> _logger;
    private readonly ContentService _contentService;

    /// <inheritdoc />
    public FiltersController(ILogger<FiltersController> logger, ContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }
    
    /// <summary>
    /// Для фильтрации контента
    /// </summary>
    /// <param name="type">Тип контента</param>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Content")]
    public async Task<ActionResult<ContentFiltersEntity>> GetFiltersForContent([Required] ContentType type)
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