using Ardalis.GuardClauses;
using HtmlAgilityPack;
using TiLostFilm.Entities.Episode;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser.Services;

public partial class EpisodeService
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

    // ReSharper disable once MemberCanBeMadeStatic.Global
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
                : ClearInnerText().Replace($"{dateReleaseRuNode.InnerText} г.", string.Empty),
            durationNode is null 
                ? null 
                : ClearInnerText().Replace(durationNode.InnerText, string.Empty).Trim(),
            ratingIMDbNode is null 
                ? null 
                : ClearInnerText().Replace(ratingIMDbNode.InnerText, string.Empty).Trim(),
            descriptionNode is null 
                ? null
                : ClearInnerText().Replace(descriptionNode.InnerText, string.Empty).Trim()
        );
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"(?:\r\n|\n|\r|\t)")]
    private static partial System.Text.RegularExpressions.Regex ClearInnerText();
}