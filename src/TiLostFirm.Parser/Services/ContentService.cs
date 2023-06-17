using RestSharp;
using TiLostFilm.Entities.Content;
using TiLostFirm.Preferences;

namespace TiLostFirm.Parser.Services;

public class ContentService
{
    private readonly RestClient _client = new();
    
    public async Task<ContentSerialsResponse?> ObtainSerials(int offset)
    {
        var request = new RestRequest(new Uri(Prefs.BaseUrl + "/ajaxik.php"), Method.Post);
        
        request.Parameters.AddParameters(new[]
        {
            new BodyParameter("act", "serial"),
            new BodyParameter("type", "search"),
            new BodyParameter("o", offset.ToString())
        });
        
        request.AddHeaders(new List<KeyValuePair<string, string>>
        {
            new("referer", "https://www.lostfilm.today/series/?type=search")
        });

        return await _client.PostAsync<ContentSerialsResponse>(request);
    }

    public async Task<ContentMoviesResponse?> ObtainMovies(int offset)
    {
        var request = new RestRequest(new Uri(Prefs.BaseUrl + "/ajaxik.php"), Method.Post);
        
        request.Parameters.AddParameters(new[]
        {
            new BodyParameter("act", "movies"),
            new BodyParameter("type", "search"),
            new BodyParameter("o", offset.ToString())
        });
        
        request.AddHeaders(new List<KeyValuePair<string, string>>
        {
            new("referer", "https://www.lostfilm.today/movies/?type=search")
        });
        
        return await _client.PostAsync<ContentMoviesResponse>(request);
    }
}