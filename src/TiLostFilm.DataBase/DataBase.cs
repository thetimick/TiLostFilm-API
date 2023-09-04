using AngleSharp;
using AngleSharp.Dom;
using LiteDB.Async;
using Microsoft.Extensions.Logging;
using TiLostFilm.DataBase.Entity;
using TiLostFirm.Preferences;

namespace TiLostFilm.DataBase;

public class DataBase
{
    public enum Type
    {
        Shedule
    }
    
    private enum Collection
    {
        Document
    }

    private readonly ILogger<DataBase> _logger;
    private readonly LiteDatabaseAsync _db = new(Prefs.DataBaseName);
    private ILiteCollectionAsync<DataBaseEntity> DataBaseCollection => _db.GetCollection<DataBaseEntity>(Collection.Document.ToString());
    private readonly BrowsingContext _browsingContext = new(Configuration.Default.WithDefaultLoader());
    
    public DataBase(ILogger<DataBase> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> FetchDocument(Type type)
    {
        _logger.LogInformation("DataBase => FetchDocument with {type}", type);
        
        var dataFromDataBase = await LoadFromDataBase(type);
        if (dataFromDataBase is not null && (DateTime.Now - dataFromDataBase.TimeStamp).Minutes <= 5)
        {
            _logger.LogInformation("DataBase => FetchDocument with {type} => Loaded from DB", type);
            return dataFromDataBase.Content;
        }

        _logger.LogInformation("DataBase => FetchDocument with {type} => Loaded from URL", type);
        
        var dataFromUrl = await LoadFromUrl(FetchUrl(type));
        await SaveToDataBase(type, dataFromUrl);
        
        return dataFromUrl;
    }

    private async Task<DataBaseEntity?> LoadFromDataBase(Type type)
    {
        return await DataBaseCollection.FindOneAsync(type.ToString());
    }

    private async Task<string> LoadFromUrl(string url)
    {
        return (await _browsingContext.OpenAsync(new Url(url))).Source.Text;
    }

    private async Task SaveToDataBase(Type type, string content)
    {
        var dbEntity = new DataBaseEntity(type, content);
        
        if (await DataBaseCollection.ExistsAsync(entity => entity.Type == type))
        {
            await DataBaseCollection.UpdateAsync(dbEntity);
        }
        else
        {
            await DataBaseCollection.InsertAsync(dbEntity);
        }
    }

    private static string FetchUrl(Type type)
    {
        return type switch
        {
            Type.Shedule => "/schedule/my_0/type_0",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}