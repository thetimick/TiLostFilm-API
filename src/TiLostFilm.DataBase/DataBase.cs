using AngleSharp;
using AngleSharp.Dom;
using LiteDB.Async;
using Microsoft.Extensions.Logging;
using TiLostFilm.DataBase.Entity;
using TiLostFirm.Preferences;

namespace TiLostFilm.DataBase;

// Nested

public partial class DataBase
{
    public enum Type
    {
        Shedule
    }
}

// Main

public partial class DataBase
{
    private readonly ILogger<DataBase> _logger;
    private readonly BrowsingContext _browsingContext = new(Configuration.Default.WithDefaultLoader());
    
    public DataBase(ILogger<DataBase> logger)
    {
        _logger = logger;
    }
}

// Public Methods

public partial class DataBase
{
    public async Task<DataBaseEntity> FetchDocument(Type type)
    {
        _logger.LogInformation("FetchDocument (Params: {Type})", type);
        
        var dataFromDataBase = await LoadFromDataBase(type);
        if (dataFromDataBase is not null && (DateTime.Now - dataFromDataBase.TimeStamp).TotalMinutes <= 5)
        {
            return dataFromDataBase;
        }
        
        var dataFromUrl = await LoadFromUrl(FetchUrl(type));
        var data = await SaveToDataBase(type, dataFromUrl);
        
        return data;
    } 
}

// Private Methods

public partial class DataBase
{
    private async Task<DataBaseEntity?> LoadFromDataBase(Type type)
    {
        _logger.LogInformation("LoadFromDataBase (Params: {Type})", type);
        
        using var db = new LiteDatabaseAsync(Prefs.DataBaseName);
        var collection = db.GetCollection<DataBaseEntity>();
        return await collection.FindOneAsync(entity => entity.Type == type);
    }

    private async Task<string> LoadFromUrl(string url)
    {
        _logger.LogInformation("LoadFromUrl (Params: {Url})", url);
        return (await _browsingContext.OpenAsync(new Url(url))).Source.Text;
    }

    private async Task<DataBaseEntity> SaveToDataBase(Type type, string content)
    {
        _logger.LogInformation("SaveToDataBase (Params: {Type}, {Content})", type, content[..10]);
        
        var dbEntity = new DataBaseEntity(type, content, DateTime.Now);

        using var db = new LiteDatabaseAsync(Prefs.DataBaseName);
        var collection = db.GetCollection<DataBaseEntity>();
        
        if (await collection.ExistsAsync(entity => entity.Type == type))
        {
            await collection.UpdateAsync(dbEntity);
        }
        else
        {
            await collection.InsertAsync(dbEntity);
        }

        return dbEntity;
    }
    
    private string FetchUrl(Type type)
    {
        var url = type switch
        {
            Type.Shedule => Prefs.BaseUrl + "/schedule/my_0/type_0",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        _logger.LogInformation("FetchUrl (Params: {Type}) => (Return: ({Url})", type, url);
        
        return url;
    }
}