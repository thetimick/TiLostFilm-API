namespace TiLostFilm.Entities.Content;

public record ContentDetailEntity
{
    public ContentDetailMeta? Meta { get; set; }
    public ContentDetailData? Data { get; set; }
}

public record ContentDetailMeta(
    string Url,
    DateTime TimeStamp
);

public record ContentDetailData
{
    public ContentDetailTitle? Title { get; set; } 
    public string? Cover { get; set; }
    public double? Rating { get; set; }
    public List<ContentDetailPhoto>? Photos { get; set; }
}

public record ContentDetailTitle
{
    public string? Ru { get; set; }
    public string? En { get; set; }
}

public record ContentDetailPhoto(
    string Square, 
    string Large
);