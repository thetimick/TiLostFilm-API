using JWT.Algorithms;
using JWT.Builder;

namespace TiLostFilm.Auth;

public class TokenService
{
    public record TokenEntity(
        string UserName
    );
    
    public string Generate(TokenEntity entity)
    {
        return JwtBuilder
            .Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret("12345")
            .AddClaims(
                new List<KeyValuePair<string, object>>
                {
                    new("UserName", entity.UserName)
                }
            )
            .Encode();
    }

    public TokenEntity Parse(string token)
    {
        return JwtBuilder
            .Create()
            .WithValidationParameters(
                options =>
                {
                    options.ValidateSignature = true;
                }
            )
            .Decode<TokenEntity>(token);
    }
}