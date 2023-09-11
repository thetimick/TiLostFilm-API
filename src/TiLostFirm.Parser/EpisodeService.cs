using Microsoft.Extensions.Logging;
using TiLostFilm.Entities.Episode;

namespace TiLostFirm.Parser;

public class EpisodeService
{
    private static class Paths
    { }

    private readonly ILogger<EpisodeService> _logger;

    public EpisodeService(ILogger<EpisodeService> logger)
    {
        _logger = logger;
    }
    
    public async Task<EpisodeEntity> GetEpisode(string url)
    {
        return new EpisodeEntity(
            "", 
            "", 
            "", "", 
            "", 
            -1, 
            "", 
            "", 
            "", 
            ""
        );
    }
}