using Microsoft.Extensions.Logging;
using TiLostFilm.Entities.Main;

namespace TiLostFirm.Parser;

public class MainService
{
    private static class Paths
    { }
    
    private readonly ILogger<MainService> _logger;

    public MainService(ILogger<MainService> logger)
    {
        _logger = logger;
    }
    
    public async Task<MainEntity> GetMain()
    {
        return new MainEntity(
            new List<MainSerial>(),
            new List<MainEpisode>(),
            new List<MainSeason>()
        );
    }
}