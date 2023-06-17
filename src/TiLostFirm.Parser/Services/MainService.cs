using Ardalis.GuardClauses;
using HtmlAgilityPack;
using TiLostFilm.Entities.Main;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser.Services;

public partial class MainService
{
    private static class Paths
    {
        public const string Left = "/html/body/div[3]/div[2]/div[1]";
        public const string Right = "/html/body/div[3]/div[2]/div[3]/div[4]";
    }

    public async Task<MainEntity> GetMain()
    {
        var html = await new HtmlWeb().LoadFromWebAsync(Prefs.BaseUrl);
        Guard.Against.Null(html);
        
        return new MainEntity(
            new List<MainSerial>(),
            new List<MainEpisode>(),
            new List<MainSeason>()
        );
    }
    
    [System.Text.RegularExpressions.GeneratedRegex(@"(?:\r\n|\n|\r|\t)")]
    private static partial System.Text.RegularExpressions.Regex ClearInnerText();
}