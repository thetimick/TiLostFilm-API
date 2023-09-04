namespace TiLostFilm.DataBase.Entity;

public class DataBaseEntity
{
    public DataBase.Type Type { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    
    public DataBaseEntity(DataBase.Type type, string content)
    {
        Type = type;
        Content = content;
    }
}