namespace TiLostFilm.Entities.Content;

public record ContentFiltersEntity
{
    public ContentFiltersMeta? Meta { get; set; }
    public ContentFiltersData? Data { get; set; }
}

public record ContentFiltersMeta(
    string Url,
    DateTime TimeStamp
);

public record ContentFiltersData
{
    public Dictionary<string, int>? Genres { get; set; }
    public Dictionary<string, int>? Years { get; set; }
    public Dictionary<string, int>? Channel { get; set; }
    public Dictionary<string, int>? Type { get; set; }
    public Dictionary<string, int>? Country { get; set; }
}