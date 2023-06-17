using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using TiLostFilm.Entities.Episode;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
[SuppressMessage("Performance", "CA1822:Пометьте члены как статические")]
[SuppressMessage("Performance", "SYSLIB1045:Преобразовать в \"GeneratedRegexAttribute\".")]
public class EpisodeService
{
    private static class Paths
    {
        public const string SeasonNumber = "/html/body/div[3]/div[2]/div[1]/div[1]/div[1]/div[2]/a[3]/text()";
        public const string EpisodeNumber = "/html/body/div[3]/div[2]/div[1]/div[1]/div[1]/div[2]/a[4]/text()";
        public const string Title = "/html/body/div[3]/div[2]/div[1]/div[1]/div[1]/div[3]/h1";
        public const string TitleOrig = "/html/body/div[3]/div[2]/div[1]/div[1]/div[1]/div[3]/div";
        public const string PosterUrl = "/html/body/div[3]/div[2]/div[1]/div[1]/div[3]/div[1]/div[2]/img";
        public const string Rating = "/html/body/div[3]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[2]";
        public const string DateReleaseRu = "/html/body/div[3]/div[2]/div[1]/div[1]/div[3]/div[4]/div[1]/span";
        public const string Duration = "/html/body/div[3]/div[2]/div[1]/div[1]/div[3]/div[4]/div[2]/text()[1]";
        public const string RatingIMDb = "/html/body/div[3]/div[2]/div[1]/div[1]/div[3]/div[4]/div[2]/text()[2]";
        public const string Description = "/html/body/div[3]/div[2]/div[1]/div[1]/div[6]/div[2]/div[1]";
    }

    private readonly ILogger<EpisodeService> _logger;

    public EpisodeService(ILogger<EpisodeService> logger)
    {
        _logger = logger;
    }
    
    public async Task<EpisodeEntity> GetEpisode(string url)
    {
        var html = await new HtmlWeb().LoadFromWebAsync(Prefs.BaseUrl + url);
        Guard.Against.Null(html);

        var seasonNumberNode = html.DocumentNode
            .SelectSingleNode(Paths.SeasonNumber);
        
        var episodeNumberNode = html.DocumentNode
            .SelectSingleNode(Paths.EpisodeNumber);

        var titleNode = html.DocumentNode
            .SelectSingleNode(Paths.Title);
        
        var titleOrigNode = html.DocumentNode
            .SelectSingleNode(Paths.TitleOrig);
        
        var posterUrlNode = html.DocumentNode
            .SelectSingleNode(Paths.PosterUrl);
        
        var ratingNode = html.DocumentNode
            .SelectSingleNode(Paths.Rating);

        var dateReleaseRuNode = html.DocumentNode
            .SelectSingleNode(Paths.DateReleaseRu);

        var durationNode = html.DocumentNode
            .SelectSingleNode(Paths.Duration);
        
        var ratingIMDbNode = html.DocumentNode
            .SelectSingleNode(Paths.RatingIMDb);

        var descriptionNode = html.DocumentNode
            .SelectSingleNode(Paths.Description);

        return new EpisodeEntity(
            seasonNumberNode.InnerText,
            episodeNumberNode.InnerText,
            titleNode.InnerText,
            titleOrigNode.InnerText,
            $"https:{posterUrlNode.Attributes["src"].Value}",
            double.Parse(ratingNode.InnerText.Replace(".", ",")),
            dateReleaseRuNode is null
                ? null
                : ClearInnerText($"{dateReleaseRuNode.InnerText} г."),
            durationNode is null
                ? null
                : ClearInnerText(durationNode.InnerText).Trim(),
            ratingIMDbNode is null
                ? null
                : ClearInnerText(ratingIMDbNode.InnerText).Trim(),
            descriptionNode is null
                ? null
                : ClearInnerText(descriptionNode.InnerText).Trim()
        );
    }

    private static string ClearInnerText(string text)
    {
        return new Regex(@"(?:\r\n|\n|\r|\t)").Replace(text, string.Empty);
    }
}