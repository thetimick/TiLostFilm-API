namespace TiLostFilm.Entities.Content;

// ReSharper disable UnusedAutoPropertyAccessor.Global

public record ContentEntity
{
    public List<ContentData>? Data { get; set; }
}

public record ContentData
{
    public long? Id { get; set; }
    public double? Rating { get; set; }
    public ContentTitle? Title { get; set; }
    public string? Year { get; set; }
    public string? Link { get; set; }
    public string? Cover { get; set; }
    public string? Genres { get; set; }
    public string? Channels { get; set; }
}

public record ContentTitle
{
    public string? Ru { get; set; }
    public string? En { get; set; }
}