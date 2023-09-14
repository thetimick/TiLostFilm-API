using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TiLostFilm.Auth;

public sealed class AuthContext : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}