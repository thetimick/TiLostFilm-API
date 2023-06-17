using Newtonsoft.Json;

// ReSharper disable ClassNeverInstantiated.Global

namespace TiLostFilm.Entities.Content;

public record ContentSerialsResponse(
    [property: JsonProperty("data")] List<SerialData>? Data,
    [property: JsonProperty("result")] string? Result
);

public record SerialData(
    [property: JsonProperty("rating")] double? Rating,
    [property: JsonProperty("title")] string? Title,
    [property: JsonProperty("title_orig")] string? TitleOrig,
    [property: JsonProperty("date")] string? Date,
    [property: JsonProperty("link")] string? Link,
    [property: JsonProperty("alias")] string? Alias,
    [property: JsonProperty("not_favorited")] bool? NotFavorited,
    [property: JsonProperty("id")] string? Id,
    [property: JsonProperty("has_icon")] string? HasIcon,
    [property: JsonProperty("has_image")] bool? HasImage,
    [property: JsonProperty("img")] string? Img,
    [property: JsonProperty("genres")] string? Genres,
    [property: JsonProperty("channels")] string? Channels,
    [property: JsonProperty("status")] string? Status,
    [property: JsonProperty("status_7")] bool? Status7,
    [property: JsonProperty("status_auto")] int? StatusAuto,
    [property: JsonProperty("status_auto_5")] bool? StatusAuto5,
    [property: JsonProperty("status_season")] string? StatusSeason,
    [property: JsonProperty("status_5")] bool? Status5,
    [property: JsonProperty("status_auto_2")] bool? StatusAuto2,
    [property: JsonProperty("status_auto_1")] bool? StatusAuto1
);