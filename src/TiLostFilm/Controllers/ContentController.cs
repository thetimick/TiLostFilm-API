using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TiLostFilm.Entities.Content;
using TiLostFilm.Entities.Error;
using TiLostFirm.Parser;

namespace TiLostFilm.Controllers;

/// <summary>
/// Контент (Сериалы, Фильмы)
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ContentController: ControllerBase
{
    private readonly ILogger<ContentController> _logger;
    private readonly ContentService _contentService;

    /// <inheritdoc />
    public ContentController(ILogger<ContentController> logger, ContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }
    
    /// <summary>
    /// Сериалы
    /// </summary>
    /// <param name="offset">Смещение</param>
    /// <param name="sort">Сортировка</param>
    /// <param name="genre">Жанр</param>
    /// <param name="year">Год</param>
    /// <param name="channel">Канал</param>
    /// <param name="type">Тип</param>
    /// <param name="country">Страна</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Serials")]
    public async Task<ActionResult<ContentEntity>> GetSerials(
        int offset = 0, 
        ContentSort sort = ContentSort.Novelty,
        [FromQuery(Name = "genre[]")] List<int>? genre = null,
        [FromQuery(Name = "year[]")] List<int>? year = null,
        [FromQuery(Name = "channel[]")] List<int>? channel = null,
        [FromQuery(Name = "type[]")] List<int>? type = null,
        [FromQuery(Name = "country[]")] List<int>? country = null
    ) {
        try
        {
            return await _contentService.ObtainSerials(offset, (int) sort, genre, year, channel, type, country);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
    
    /// <summary>
    /// Фильмы
    /// </summary>
    /// <param name="offset">Смещение</param>
    /// <param name="sort">Сортировка</param>
    /// <param name="genre">Жанр</param>
    /// <param name="year">Год</param>
    /// <param name="type">Тип</param>
    /// <param name="country">Страна</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Movies")]
    public async Task<ActionResult<ContentEntity>> GetMovies(
        int offset = 0, 
        ContentSort sort = ContentSort.Novelty,
        [FromQuery(Name = "genre[]")] List<int>? genre = null,
        [FromQuery(Name = "year[]")] List<int>? year = null,
        [FromQuery(Name = "type[]")] List<int>? type = null,
        [FromQuery(Name = "country[]")] List<int>? country = null
    ) {
        try
        {
            return await _contentService.ObtainMovies(offset, (int) sort, genre, year, type, country);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
    
    /// <summary>
    /// Деталка
    /// </summary>
    /// <param name="url">Путь (Без BaseURL (/serials/...))</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorEntity))]
    [HttpGet("Detail")]
    public async Task<ActionResult<ContentDetailEntity>> GetDetail([Required] string url)
    {
        try
        {
            return await _contentService.ObtainDetail(url);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {ExMessage}", ex.Message);
            return new BadRequestObjectResult(new ErrorEntity(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}