using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using TiLostFilm.Cache;
using TiLostFilm.Entities.Shedule;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser;

public partial class SheduleService
{
    private static class Paths
    {
        public const string Url = Prefs.BaseUrl + "/schedule/my_0/type_0";
        public const string Body = "#left-pane > div > div.content > div > table > tbody";    
    } 
}

public partial class SheduleService
{
    private readonly ILogger<SheduleService> _logger;
    private readonly CacheService _cacheService;

    public SheduleService(ILogger<SheduleService> logger, CacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<SheduleEntity> GetShedule()
    {
        _logger.LogInformation("GetShedule");

        var data = await _cacheService.LoadAsync(Paths.Url);
        Guard.Against.Null(data);
        
        var document = await BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(data.Source));
        
        var entity = new SheduleEntity(
            new SheduleMeta(data.Url, DateTime.Parse(data.TimeStamp)),
            new SheduleData(
                new List<SheduleDataItem>(),
                new List<SheduleDataItem>(),
                new List<SheduleDataItem>(),
                new List<SheduleDataItem>(),
                new List<SheduleDataItem>()
            )
        );
        
        var body = document.QuerySelector(Paths.Body)?.ChildNodes;
        Guard.Against.Null(body);
        
        var sectionIndex = -1;
        foreach (var section in body)
        {
            if (!section.HasChildNodes)
            {
                continue;
            }
            
            if (section.ChildNodes.Any(node => node.NodeName == "TH"))
            {
                sectionIndex++;
                continue;
            }

            var sheduleDataItem = ParseFromNode(section);
            if (sheduleDataItem is null)
            {
                continue;
            }
                
            switch (sectionIndex)
            {
                case 0:
                    entity.Data?.Today?.Add(sheduleDataItem);
                    break;
                    
                case 1:
                    entity.Data?.Tomorrow?.Add(sheduleDataItem);
                    break;
                    
                case 2:
                    entity.Data?.OnThisWeek?.Add(sheduleDataItem);
                    break;
                    
                case 3:
                    entity.Data?.OnNextWeek?.Add(sheduleDataItem);
                    break;
                    
                case 4:
                    entity.Data?.Future?.Add(sheduleDataItem);
                    break;
            }
        }
        
        return entity;
    }
}

#region Private Methods
public partial class SheduleService
{
    private static SheduleDataItem? ParseFromNode(INode node) 
    {
        var cover = node.ChildNodes.QuerySelector("td.alpha > div > div.cover > img")?.Attributes["src"]?.Value;
        if (cover is null)
        {
            return null;
        }
        
        var titleRu = node.ChildNodes.QuerySelector("td.alpha > div > div.title-block > div.ru")?.Text();
        var titleEn = node.ChildNodes.QuerySelector("td.alpha > div > div.title-block > div.en.small-text")?.Text();
        var episodeNumber = node.ChildNodes.QuerySelector("td.beta > div > div")?.Text();
        var episodeTitleEn = node.ChildNodes.QuerySelector("td.gamma")?.Text();
        var releaseDate = node.ChildNodes.QuerySelector("td.delta")?.Text();
        
        return new SheduleDataItem(
            $"https:{cover}",
            new SheduleSerialTitle(titleRu, titleEn),
            ParseEpisodeNumber(episodeNumber),
            new SheduleEpisodeTitle(null, episodeTitleEn),
            ParseReleaseDate(releaseDate)
        );
    }
    
    private static SheduleEpisodeNumber? ParseEpisodeNumber(string? input)
    {
        if (input is null)
        {
            return null;
        }
        
        const string pattern = @"(\d+)\s+сезон\s+(\d+)\s+серия";

        #pragma warning disable SYSLIB1045
        var match = Regex.Match(input, pattern);
        #pragma warning restore SYSLIB1045
        
        if (match.Success)
        {
            return new SheduleEpisodeNumber(
                int.Parse(match.Groups[1].Value), 
                int.Parse(match.Groups[2].Value)
            );
        }

        return null;
    }
    
    private static DateOnly? ParseReleaseDate(string? input)
    {
        if (input is null)
        {
            return null;
        }
        
        const string pattern = @"\w+, (\d{2}.\d{2}.\d{4})";

        #pragma warning disable SYSLIB1045
        var match = Regex.Match(input, pattern);
        #pragma warning restore SYSLIB1045
        
        // ReSharper disable once InvertIf
        if (match.Success)
        {
            var dateString = match.Groups[1].Value;
            
            if (DateOnly.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
        }

        return null;
    }
}
#endregion