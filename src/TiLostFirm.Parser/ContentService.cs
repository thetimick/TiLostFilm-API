using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using TiLostFilm.Entities.Content;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser;

public class ContentService
{
    private readonly ILogger<ContentService> _logger;
    private readonly RestClient _client = new(
        configureSerialization: s => s.UseNewtonsoftJson()
    );

    public ContentService(ILogger<ContentService> logger)
    {
        _logger = logger;
    }
    
    public async Task<RestResponse<ContentSerialsResponse>> ObtainSerials(int offset)
    {
        var request = new RestRequest(new Uri(Prefs.BaseUrl + "/ajaxik.php"), Method.Post);
        
        request.AddOrUpdateParameter("act", "serial", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("type", "search", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("o", offset.ToString(), ParameterType.GetOrPost);
        
        request.AddHeader("referer", "https://www.lostfilm.today/serial/?type=search");
        
        return (await _client.ExecuteAsync<ContentSerialsResponse>(request)).ThrowIfError();
    }

    public async Task<RestResponse<ContentMoviesResponse>> ObtainMovies(int offset)
    {
        var request = new RestRequest(new Uri(Prefs.BaseUrl + "/ajaxik.php"), Method.Post);
        
        request.AddOrUpdateParameter("act", "movies", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("type", "search", ParameterType.GetOrPost);
        request.AddOrUpdateParameter("o", offset.ToString(), ParameterType.GetOrPost);
        
        request.AddHeader("referer", "https://www.lostfilm.today/movies/?type=search");
        
        return (await _client.ExecuteAsync<ContentMoviesResponse>(request)).ThrowIfError();
    }
}