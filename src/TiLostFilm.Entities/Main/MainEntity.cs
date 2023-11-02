namespace TiLostFilm.Entities.Main;

// ReSharper disable ClassNeverInstantiated.Global

public record MainEntity
{
    public List<MainMeta>? Meta { get; set; }
    public MainData? Data { get; set; }
}

public record MainMeta(
    string? Url,
    DateTime? TimeStamp
);

public record MainData(
    List<MainNews>? News,
    List<MainVideo>? Video
);

public record MainNews(
    string? PosterUrl,
    string? Title,
    string? Description,
    string? Date
);

public record MainVideo(
    string? PosterUrl,
    List<MainVideoUrl>? VideoUrl,
    string? Title,
    string? Description
);

public record MainVideoUrl(
    string? Quality,
    string? Url
);