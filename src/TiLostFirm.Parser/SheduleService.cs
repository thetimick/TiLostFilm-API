using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using TiLostFilm.Entities.Shedule;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser;

public class SheduleService
{
    private static class Paths
    {
        public const string Body = "#left-pane > div > div.content > div > table > tbody";    
    }
    
    private readonly ILogger<SheduleService> _logger;

    public SheduleService(ILogger<SheduleService> logger)
    {
        _logger = logger;
    }

    public async Task<SheduleEntity> GetShedule()
    {
        _logger.LogInformation("GetShedule");

        var context = new BrowsingContext(Configuration.Default.WithDefaultLoader());
        
        var document = await context.OpenAsync(new Url(Prefs.BaseUrl + "/schedule/my_0/type_0"));
        Guard.Against.Null(document);
        
        var entity = new SheduleEntity(
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

            foreach (var item in section.ChildNodes)
            {
                var cover = item.ChildNodes.QuerySelector("td.alpha > div > div.cover > img")?.Attributes["src"]?.Value;
                if (cover == null)
                {
                    continue;
                }
                var titleRu = item.ChildNodes.QuerySelector("td.alpha > div > div.title-block > div.ru")?.Text();
                var titleEn = item.ChildNodes.QuerySelector("td.alpha > div > div.title-block > div.en.small-text")?.Text();
                var episodeTitleEn = item.ChildNodes.QuerySelector("td.gamma")?.Text();
                var releaseDate = item.ChildNodes.QuerySelector("td.delta")?.Text();
                
                var sheduleDataItem = new SheduleDataItem(
                    $"https:{cover}",
                    new SheduleSerialTitle(titleRu, titleEn),
                    new SheduleEpisodeNumber(-1, -1),
                    new SheduleEpisodeTitle(null, episodeTitleEn),
                    releaseDate
                );

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
        }
        
        return entity;
    }
    
    private static string ClearInnerText(string text)
    {
        return new Regex(@"(?:\r\n|\n|\r|\t)").Replace(text, string.Empty);
    }
}