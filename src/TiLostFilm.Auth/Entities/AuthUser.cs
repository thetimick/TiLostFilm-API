using Microsoft.AspNetCore.Identity;

namespace TiLostFilm.Auth.Entities;

public class AuthUser: IdentityUser<long>
{
    public string? Session {  get; set; }
}

