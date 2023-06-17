using System.Text.Json.Serialization;

// ReSharper disable ClassNeverInstantiated.Global

namespace TiLostFilm.Entities.Content;

public record ContentSerialsResponse(
    [property: JsonPropertyName("data")] IReadOnlyList<SerialData> Data,
    [property: JsonPropertyName("result")] string Result
);
    
public record SerialData(
    [property: JsonPropertyName("rating")] double? Rating,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("title_orig")] string TitleOrig,
    [property: JsonPropertyName("date")] string Date,
    [property: JsonPropertyName("link")] string Link,
    [property: JsonPropertyName("alias")] string Alias,
    [property: JsonPropertyName("not_favorited")] bool? NotFavorited,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("has_icon")] string HasIcon,
    [property: JsonPropertyName("has_image")] bool? HasImage,
    [property: JsonPropertyName("img")] string Img,
    [property: JsonPropertyName("genres")] string Genres,
    [property: JsonPropertyName("channels")] string Channels,
    [property: JsonPropertyName("status")] object Status,
    [property: JsonPropertyName("status_7")] bool? Status7,
    [property: JsonPropertyName("status_auto")] int? StatusAuto,
    [property: JsonPropertyName("status_auto_5")] bool? StatusAuto5,
    [property: JsonPropertyName("status_season")] string StatusSeason,
    [property: JsonPropertyName("status_5")] bool? Status5,
    [property: JsonPropertyName("status_auto_2")] bool? StatusAuto2,
    [property: JsonPropertyName("status_auto_1")] bool? StatusAuto1
);