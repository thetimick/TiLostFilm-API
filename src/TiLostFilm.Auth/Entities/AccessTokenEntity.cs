namespace TiLostFilm.Auth.Entities;

public record AccessTokenEntity
{
    public AccessTokenData? Data { get; set; } 
}

public record AccessTokenData
{
    public string? AccessToken { get; set; }
}