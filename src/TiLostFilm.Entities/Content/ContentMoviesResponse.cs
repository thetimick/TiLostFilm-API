using System.Text.Json.Serialization;

// ReSharper disable ClassNeverInstantiated.Global

namespace TiLostFilm.Entities.Content;

public record ContentMoviesResponse(
    [property: JsonPropertyName("data")] IReadOnlyList<MovieData> Data,
    [property: JsonPropertyName("result")] string Result
);

public record MovieData(
    [property: JsonPropertyName("rating")] double? Rating,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("title_orig")] string TitleOrig,
    [property: JsonPropertyName("date")] string Date,
    [property: JsonPropertyName("link")] string Link,
    [property: JsonPropertyName("ismovie")] string IsMovie,
    [property: JsonPropertyName("alias")] string Alias,
    [property: JsonPropertyName("not_favorited")] bool? NotFavorited,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("has_icon")] string HasIcon,
    [property: JsonPropertyName("has_image")] bool? HasImage,
    [property: JsonPropertyName("img")] string Img,
    [property: JsonPropertyName("genres")] string Genres,
    [property: JsonPropertyName("channels")] string Channels
);
