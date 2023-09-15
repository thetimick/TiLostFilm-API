using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using TiLostFilm.Auth.Entities;

namespace TiLostFilm.Auth;

public partial class AuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly AuthContext _authContext;
    private readonly TokenService _tokenService;
    
    private readonly RestClient _client = new(configureSerialization: s => s.UseNewtonsoftJson());
    private readonly string _baseUrl;
    
    public AuthService(
        ILogger<AuthService> logger, 
        AuthContext authContext, 
        Microsoft.Extensions.Configuration.IConfiguration configuration, 
        TokenService tokenService
    ) {
        _logger = logger;
        _authContext = authContext;
        _baseUrl = configuration["App:BaseUrl"] ?? "";
        _tokenService = tokenService;
    }
}

// Public Methods

public partial class AuthService
{
    public async Task<AccessTokenEntity> LoginAsync(LoginRequestEntity entity)
    {
        _logger.LogInformation("LoginAsync");

        var response = await ObtainLoginAsync(entity);
        if (response.Data?.Success is null or false)
            throw new UnauthorizedAccessException("Неверный Логин/Пароль");

        var userName = response.Data.Name;
        var cookie = response.Cookies?["lf_session"]?.Value;
        if (userName is not null or "" && cookie is not null or "")
            await SaveToDataBaseAsync(userName, cookie);
        else 
            throw new UnauthorizedAccessException("Неверный Логин/Пароль");

        return new AccessTokenEntity
        {
            Data = new AccessTokenData
            {
                AccessToken = _tokenService.Generate(new TokenService.TokenEntity(response.Data.Name ?? ""))
            }
        };
    }

    public async Task<string?> FetchSessionAsync(string token)
    {
        try
        {
            var entity = _tokenService.Parse(token);
            var user = (await _authContext.Users.ToListAsync()).Find(user => user.UserName == entity.UserName);
            return user?.Session;
        }
        catch
        {
            return null;
        }
    }
}

// Private Methods

public partial class AuthService
{
    private async Task<RestResponse<LoginResponse>> ObtainLoginAsync(LoginRequestEntity entity)
    {
        var request = new RestRequest(new Uri(_baseUrl + "/ajaxik.users.php"), Method.Post);
        
        request.AddOrUpdateParameters(
            new List<Parameter>
            {
                new GetOrPostParameter("act", "users"),
                new GetOrPostParameter("type", "login"),
                new GetOrPostParameter("mail", entity.Login),
                new GetOrPostParameter("pass", entity.Password),
                new GetOrPostParameter("rem", "1")
            }
        );
        
        return (await _client.ExecuteAsync<LoginResponse>(request)).ThrowIfError();
    }
    
    private static Dictionary<string, string> GetCookieDictionary(string cookies)
    {
        var cookieDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        var values = cookies.TrimEnd(';').Split(';');
        foreach (var parts in values.Select(c => c.Split(new[] { '=' }, 2)))
        {
            var cookieName = parts[0].Trim();
            var cookieValue = parts.Length == 1 ? string.Empty : parts[1];
            
            cookieDictionary[cookieName] = cookieValue;
        }

        return cookieDictionary;
    }

    private async Task SaveToDataBaseAsync(string userName, string session)
    {
        await _authContext.Users.AddAsync(
            new AuthUser
            {
                UserName = userName,
                Session = session
            }
        );

        await _authContext.SaveChangesAsync();
    }
}