using Microsoft.EntityFrameworkCore;
using TiLostFilm.Entities.Cache;

namespace TiLostFilm.Cache.Context;

public sealed class CacheContext: DbContext
{
    public DbSet<CacheEntity> CacheEntity => Set<CacheEntity>();
    
    public CacheContext(DbContextOptions<CacheContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<CacheEntity>()
            .HasKey(entity => entity.Url);
        
        base.OnModelCreating(modelBuilder);
    }
}