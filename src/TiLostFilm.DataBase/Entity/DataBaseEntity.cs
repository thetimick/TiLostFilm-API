namespace TiLostFilm.DataBase.Entity;

public class DataBaseEntity
{
    public DataBase.Type Type { get; }
    public string Content { get; }
    public DateTime TimeStamp { get; }
    
    public DataBaseEntity(DataBase.Type type, string content, DateTime timeStamp)
    {
        Type = type;
        Content = content;
        TimeStamp = timeStamp;
    }
}