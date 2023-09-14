using Microsoft.AspNetCore.Identity;
using TiLostFilm.Auth.Entities;

namespace TiLostFilm.Auth;

public class AuthService
{
    private readonly AuthContext _authContext;

    public AuthService(AuthContext authContext)
    {
        _authContext = authContext;
    }

    public async Task Login(LoginRequestEntity entity)
    {
        await _authContext.Users.AddAsync(new IdentityUser<long>(entity.Login));
        await _authContext.SaveChangesAsync();
    }
}