using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TiLostFilm.Auth.Entities;

namespace TiLostFilm.Auth;

public sealed class AuthContext : IdentityDbContext<AuthUser, IdentityRole<long>, long>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}