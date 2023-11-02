using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Microsoft.Extensions.Logging;
using TiLostFilm.Cache;
using TiLostFilm.Entities.Cache;
using TiLostFilm.Entities.Main;

namespace TiLostFirm.Parser;

public partial class MainService
{
    private static class Paths
    { }
    
    private readonly ILogger<MainService> _logger;
    private readonly CacheService _cacheService;
    private readonly string _baseUrl;

    public MainService(Microsoft.Extensions.Configuration.IConfiguration configuration, ILogger<MainService> logger, CacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
        _baseUrl = configuration["App:BaseUrl"] ?? "";
    }
    
    public async Task<MainEntity> GetMain()
    {
        _logger.LogInformation("GetMain");
        
        var news = await _cacheService.LoadAsync(_baseUrl + "/news");
        var video = await _cacheService.LoadAsync(_baseUrl + "/video");
        
        return new MainEntity
        {
            Meta = new List<MainMeta>
            {
                new(news?.Url, DateTime.Parse(news?.TimeStamp ?? "00:00:00")),
                new(video?.Url, DateTime.Parse(video?.TimeStamp ?? "00:00:00"))
            },
            Data = new MainData(
                await GetNews(news),
                await GetVideo(video)
            )
        };
    }
}

#region Private Static Methods
public partial class MainService
{
    private static async Task<List<MainNews>?> GetNews(CacheEntity? data)
    {
        if (data is null)
            return null;
        
        var document = await BrowsingContext
            .New(Configuration.Default)
            .OpenAsync(req => req.Content(data.Source));

        var body = document
            .QuerySelector("#left-pane > div.content > div > div.text-block.news-box > div.body")
            ?.ChildNodes
            .Where(node => node.NodeName == "A")
            .Take(5);

        return body
            ?.Select(ParseNewsFromNode)
            .ToList();
    }
    
    private static MainNews ParseNewsFromNode(INode node)
    {
        var poster = node.ChildNodes.QuerySelector("div.image > img.thumb")?.Attributes["src"]?.Value;
        
        return new MainNews(
            poster is null ? null : $"https:{poster}", 
            node.ChildNodes.QuerySelector("div.body > div.news-title")?.Text(), 
            node.ChildNodes.QuerySelector("div.body > div.news-text")?.Text(), 
            node.ChildNodes.QuerySelector("div.body > div.news-date")?.Text().Trim()
        );
    }
    
    private static async Task<List<MainVideo>?> GetVideo(CacheEntity? data)
    {
        if (data is null)
            return null;
        
        var document = await BrowsingContext
            .New(Configuration.Default)
            .OpenAsync(req => req.Content(data.Source));

        var body = document
            .QuerySelector("#left-pane > div.content")
            ?.ChildNodes
            .Where(node => node.NodeName == "DIV" && (node as IHtmlDivElement)?.ClassName == "video-block video_block");

        return body
            ?.Select(ParseVideoFromNode)
            .ToList();
    } 
    
    private static MainVideo ParseVideoFromNode(INode node)
    {
        var poster = node.ChildNodes.QuerySelector("img")?.Attributes["src"]?.Value;

        var videoElement = node
            .ChildNodes
            .QuerySelector("div.play-btn");
        
        var videoUrl = videoElement
            ?.Attributes["data-src"]
            ?.Value;
        
        var quality = videoElement
            ?.Attributes["data-quality"]
            ?.Value
            .Split(",");
        
        var videoUrlList = quality
            ?.Select(q => new MainVideoUrl(q, videoUrl?.Replace("720p", q)))
            .ToList();
        
        return new MainVideo(
            poster is null ? null : $"https:{poster}",
            videoUrlList,
            node.ChildNodes.QuerySelector("div.bottom-pane > div.title")?.Text(),
            node.ChildNodes.QuerySelector("div.bottom-pane > div.description")?.Text()
        );
    }
}
#endregion