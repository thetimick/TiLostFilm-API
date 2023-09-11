using System.Globalization;
using TiLostFilm.Cache.Context;
using TiLostFilm.Entities.Cache;

namespace TiLostFilm.Cache;

public class CacheService
{
    private readonly CacheContext _db;
    
    public CacheService(CacheContext db)
    {
        _db = db;
    }
    
    public async Task<CacheEntity?> LoadAsync(string url)
    {
        var entityFromDataBase = _db.CacheEntity
            .ToList()
            .FirstOrDefault(entity => entity.Url == url);
        
        if (entityFromDataBase != null && (DateTime.Now - DateTime.Parse(entityFromDataBase.TimeStamp)).TotalMinutes <= 5)
            return entityFromDataBase;
        
        var source = await WebService.LoadAsync(url);
        return await SaveAsync(url, source);
    }
    
    private async Task<CacheEntity> SaveAsync(string url, string source)
    {
        var entityFromCache = _db.CacheEntity
            .ToList()
            .FirstOrDefault(entity => entity.Url == url);
        
        if (entityFromCache != null)
        {
            entityFromCache.Source = source;
            entityFromCache.TimeStamp = DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
            
            _db.CacheEntity.Update(entityFromCache);
            await _db.SaveChangesAsync();
            return entityFromCache;
        }
        
        var entity = new CacheEntity(source, url, DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture));
        await _db.CacheEntity.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}