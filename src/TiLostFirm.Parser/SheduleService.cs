using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using TiLostFilm.DataBase;
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
    private readonly DataBase _db;

    public SheduleService(ILogger<SheduleService> logger, DataBase db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<SheduleEntity> GetShedule()
    {
        _logger.LogInformation("SheduleService => GetShedule");

        var data = await _db.FetchDocument(DataBase.Type.Shedule);
        var document = await BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(data));
        
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
}