using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using TiLostFilm.Entities.Main;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser;

[SuppressMessage("Performance", "SYSLIB1045:Преобразовать в \"GeneratedRegexAttribute\".")]
[SuppressMessage("Performance", "CA1822:Пометьте члены как статические")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class MainService
{
    private static class Paths
    {
        public const string NewEpisodes = "/html/body/div[3]/div[2]/div[3]/div[4]/div[3]";
    }
    
    private readonly ILogger<MainService> _logger;

    public MainService(ILogger<MainService> logger)
    {
        _logger = logger;
    }
    
    public async Task<MainEntity> GetMain()
    {
        var html = await new HtmlWeb().LoadFromWebAsync(Prefs.BaseUrl);
        Guard.Against.Null(html);
        
        return new MainEntity(
            new List<MainSerial>(),
            GetNewEpisodes(html.DocumentNode.SelectSingleNode(Paths.NewEpisodes).ChildNodes),
            new List<MainSeason>()
        );
    }

    private List<MainEpisode> GetNewEpisodes(HtmlNodeCollection collection)
    {
        return new List<MainEpisode>();
    }
    
    private static string ClearInnerText(string text)
    {
        return new Regex(@"(?:\r\n|\n|\r|\t)").Replace(text, string.Empty);
    }
}