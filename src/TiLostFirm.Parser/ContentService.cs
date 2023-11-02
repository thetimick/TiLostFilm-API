using System.Globalization;
using AngleSharp;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using TiLostFilm.Cache;
using TiLostFilm.Entities.Content;

using ContentType = TiLostFilm.Entities.Content.ContentType;

namespace TiLostFirm.Parser;

public partial class ContentService
{
    private static class Paths
    {
        public enum ContentDetailType
        {
            Base,
            Photos
        }
        
        public static string FetchUrl(string baseUrl, ContentType contentType)
        {
            return contentType switch
            {
                ContentType.Serials => baseUrl + "/series",
                ContentType.Movies => baseUrl + "/movies",
                _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null)
            };
        }

        public static string FetchUrlForDetail(string baseUrl, string url, ContentDetailType type)
        {
            return type switch
            {
                ContentDetailType.Base => baseUrl + url,
                ContentDetailType.Photos => baseUrl + url + "/photos",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static class Selectors
        {
            public const string GenreBlock = "#left-pane > div.content > div.left-side-block > div:nth-child(2)";
            public const string YearBlock = "#left-pane > div.content > div.left-side-block > div:nth-child(4)";
            public const string ChannelBlock = "#left-pane > div.content > div.left-side-block > div:nth-child(6)";
            public const string TypeBlock = "#left-pane > div.content > div.left-side-block > div:nth-child(8)";
            public const string CountryBlock = "#left-pane > div.content > div.left-side-block > div:nth-child(10)";
        }
    } 
}

public partial class ContentService
{
    private readonly ILogger<ContentService> _logger;
    private readonly CacheService _cacheService;
    private readonly RestClient _client = new(configureSerialization: s => s.UseNewtonsoftJson());
    private readonly string _baseUrl;

    public ContentService(Microsoft.Extensions.Configuration.IConfiguration configuration, ILogger<ContentService> logger, CacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
        _baseUrl = configuration["App:BaseUrl"] ?? "";
    }
    
    public async Task<ContentEntity> ObtainSerials(
        int offset, 
        int? sort = null,
        List<int>? genre = null,
        List<int>? year = null,
        List<int>? channel = null,
        List<int>? type = null,
        List<int>? country = null
    ) {
        _logger.LogInformation("ObtainSerials");
        
        var request = new RestRequest(new Uri(_baseUrl + "/ajaxik.php"), Method.Post);
        
        request.AddOrUpdateParameter("act", "serial", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("type", "search", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("o", offset.ToString(), ParameterType.GetOrPost);

        if (sort != null)
            request.AddOrUpdateParameter("s", sort, ParameterType.GetOrPost);
        
        if (genre != null)
            request.AddOrUpdateParameter("g", string.Join(',', genre), ParameterType.GetOrPost);
        
        if (year != null)
            request.AddOrUpdateParameter("y", string.Join(',', year), ParameterType.GetOrPost);
        
        if (channel != null)
            request.AddOrUpdateParameter("c", string.Join(',', channel), ParameterType.GetOrPost);
        
        if (type != null)
            request.AddOrUpdateParameter("r", string.Join(',', type), ParameterType.GetOrPost);
        
        if (country != null)
            request.AddOrUpdateParameter("cntr", string.Join(',', country), ParameterType.GetOrPost);
        
        request.AddHeader("referer", "https://www.lostfilm.today/serial/?type=search");
        
        var response = (await _client.ExecuteAsync<ContentSerialsResponse>(request)).ThrowIfError();

        return new ContentEntity
        {
            Data = new List<ContentData>(
                response.Data?.Data?.Select(
                    data => new ContentData
                    {
                        Id = long.Parse(data.Id ?? "-1"),
                        Title = new ContentTitle
                        {
                            Ru = data.Title,
                            En = data.TitleOrig
                        },
                        Rating = data.Rating,
                        Year = data.Date?.ToString(),
                        Link = data.Link,
                        Cover = $"https:{data.Img}",
                        Genres = data.Genres,
                        Channels = data.Channels
                    }
                ) ?? new List<ContentData>()
            )
        };
    }

    public async Task<ContentEntity> ObtainMovies(
        int offset, 
        int? sort = null,
        List<int>? genre = null,
        List<int>? year = null,
        List<int>? type = null,
        List<int>? country = null
    ) {
        _logger.LogInformation("ObtainMovies");
        
        var request = new RestRequest(new Uri(_baseUrl + "/ajaxik.php"), Method.Post);
        
        request.AddOrUpdateParameter("act", "movies", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("type", "search", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("o", offset.ToString(), ParameterType.GetOrPost);
        
        if (sort != null)
            request.AddOrUpdateParameter("s", sort, ParameterType.GetOrPost);
        
        if (genre != null)
            request.AddOrUpdateParameter("g", string.Join(',', genre), ParameterType.GetOrPost);
        
        if (year != null)
            request.AddOrUpdateParameter("y", string.Join(',', year), ParameterType.GetOrPost);
        
        if (type != null)
            request.AddOrUpdateParameter("r", string.Join(',', type), ParameterType.GetOrPost);
        
        if (country != null)
            request.AddOrUpdateParameter("cntr", string.Join(',', country), ParameterType.GetOrPost);
        
        request.AddHeader("referer", "https://www.lostfilm.today/movies/?type=search");
        
        var response = (await _client.ExecuteAsync<ContentMoviesResponse>(request)).ThrowIfError();
        
        return new ContentEntity
        {
            Data = new List<ContentData>(
                response.Data?.Data?.Select(
                    data => new ContentData
                    {
                        Id = long.Parse(data.Id ?? "-1"),
                        Title = new ContentTitle
                        {
                            Ru = data.Title,
                            En = data.TitleOrig
                        },
                        Rating = data.Rating,
                        Year = data.Date?.ToString(),
                        Link = data.Link,
                        Cover = $"https:{data.Img}",
                        Genres = data.Genres,
                        Channels = data.Channels
                    }
                ) ?? new List<ContentData>()
            )
        };
    }

    public async Task<ContentFiltersEntity> ObtainFilters(ContentType contentType)
    {
        _logger.LogInformation("ObtainFilters");
        
        var data = await _cacheService.LoadAsync(Paths.FetchUrl(_baseUrl, contentType));
        Guard.Against.Null(data);
        
        var document = await BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(data.Source));
        Guard.Against.Null(document);
        
        var entity = new ContentFiltersEntity
        {
            Meta = new ContentFiltersMeta(data.Url, DateTime.Parse(data.TimeStamp)),
            Data = new ContentFiltersData()
        };
        
        var genreBlock = document.QuerySelector(Paths.Selectors.GenreBlock);
        if (genreBlock != null)
            entity.Data.Genres = ContentFilterParser(genreBlock.ChildNodes, "genre");

        var yearBlock = document.QuerySelector(Paths.Selectors.YearBlock);
        if (yearBlock != null)
            entity.Data.Years = ContentFilterParser(yearBlock.ChildNodes, "year");

        switch (contentType)
        {
            case ContentType.Serials:
                var channelBlock = document.QuerySelector(Paths.Selectors.ChannelBlock);
                if (channelBlock != null)
                    entity.Data.Channel = ContentFilterParser(channelBlock.ChildNodes, "channel");
                
                var typeBlock = document.QuerySelector(Paths.Selectors.TypeBlock);
                if (typeBlock != null)
                    entity.Data.Type = ContentFilterParser(typeBlock.ChildNodes, "group");
        
                var countryBlock = document.QuerySelector(Paths.Selectors.CountryBlock);
                if (countryBlock != null)
                    entity.Data.Country = ContentFilterParser(countryBlock.ChildNodes, "country");  
                
                break;
            
            case ContentType.Movies:
                var typeMovieBlock = document.QuerySelector("#left-pane > div.content > div.left-side-block > div:nth-child(6)");
                if (typeMovieBlock != null)
                    entity.Data.Type = ContentFilterParser(typeMovieBlock.ChildNodes, "group");
    
                var countryMovieBlock = document.QuerySelector("#left-pane > div.content > div.left-side-block > div:nth-child(8)");
                if (countryMovieBlock != null)
                    entity.Data.Country = ContentFilterParser(countryMovieBlock.ChildNodes, "country");  
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }
        
        return entity;
    }

    public async Task<ContentDetailEntity> ObtainDetail(string url)
    {
        _logger.LogInformation("ObtainDetail ({Url})", url);
        
        var data = await _cacheService.LoadAsync(Paths.FetchUrlForDetail(_baseUrl, url, Paths.ContentDetailType.Base));
        var dataForPhotos = await _cacheService.LoadAsync(Paths.FetchUrlForDetail(_baseUrl, url, Paths.ContentDetailType.Photos));
        
        Guard.Against.Null(data);
        Guard.Against.Null(dataForPhotos);
        
        var document = await BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(data.Source));
        var documentForPhotos = await BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(dataForPhotos.Source));
        
        Guard.Against.Null(document);
        Guard.Against.Null(documentForPhotos);

        var coverBlock = document.QuerySelector("#left-pane > div > div:nth-child(5) > div.image-block > div.main_poster > img") ??
                         document.QuerySelector("#left-pane > div:nth-child(1) > div:nth-child(6) > div.image-block > div.main_poster > img");

        var entity = new ContentDetailEntity
        {
            Meta = new ContentDetailMeta(data.Url, DateTime.Parse(data.TimeStamp)),
            Data = new ContentDetailData
            {
                Title = new ContentDetailTitle
                {
                    Ru = document.QuerySelector("#left-pane > div > div:nth-child(1) > div.header > h1")?.Text(),
                    En = document.QuerySelector("#left-pane > div > div:nth-child(1) > div.header > h2")?.Text()
                },
                Cover = $"https:{coverBlock?.Attributes["src"]?.Value}",
                Rating = double.Parse(
                    document
                        .QuerySelector(
                            "#left-pane > div > div:nth-child(5) > div.image-block > div.overlay-pane > div.serie-mark-pane"
                        )
                        ?.Text() ?? "-1",
                    CultureInfo.InvariantCulture
                )
            }
        };

        var photosBlock = documentForPhotos.QuerySelector("#gallery_main");
        if (photosBlock is null) 
            return entity;
        
        entity.Data.Photos = new List<ContentDetailPhoto>();

        foreach (var node in photosBlock.ChildNodes)
        {
            var element = node.ChildNodes.QuerySelector("img");
            if (element is null) 
                continue;

            var photoUrl = $"https:{element.Attributes["src"]?.Value}";
                
            entity.Data.Photos.Add(new ContentDetailPhoto(photoUrl, photoUrl.Replace("t_", "")));
        }

        return entity;
    }
}

public partial class ContentService
{
    private static Dictionary<string, int> ContentFilterParser(INodeList nodes, string replaceCharacter)
    {
        var dictionary = new Dictionary<string, int>();
        
        foreach (var node in nodes)
        {
            if (node.Text().Contains("\n\t"))
                continue;

            var key = node.Text().Trim();
            var value = int
                .Parse(
                    (node as IElement)?.Attributes["rel"]?.Value.Replace($"{replaceCharacter}_", "") ?? "-1"
                );
            
            if (key is "" || value is -1)
                continue;

            if (dictionary.ContainsKey(key))
                key = $"{key} {new Random(Environment.TickCount).Next(100, 999)}";
            
            dictionary.Add(key, value);
        }

        return dictionary;
    }
}