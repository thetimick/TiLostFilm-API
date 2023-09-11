using System.Globalization;

namespace TiLostFilm.Entities.Cache;

public class CacheEntity
{
    public string Source { get; set; }
    public string Url { get; }
    public string TimeStamp { get; set; }

    public CacheEntity()
    {
        Source = "";
        Url = "";
        TimeStamp = DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
    }
    
    public CacheEntity(string source, string url, string timeStamp)
    {
        Source = source;
        Url = url;
        TimeStamp = timeStamp;
    }
}