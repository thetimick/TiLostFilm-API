using Newtonsoft.Json;

// ReSharper disable ClassNeverInstantiated.Global

namespace TiLostFilm.Entities.Content;

public record ContentMoviesResponse(
    [property: JsonProperty("data")] List<MovieData>? Data,
    [property: JsonProperty("result")] string? Result
);

public record MovieData(
    [property: JsonProperty("rating")] double? Rating,
    [property: JsonProperty("title")] string? Title,
    [property: JsonProperty("title_orig")] string? TitleOrig,
    [property: JsonProperty("date")] string? Date,
    [property: JsonProperty("link")] string? Link,
    [property: JsonProperty("ismovie")] string? IsMovie,
    [property: JsonProperty("alias")] string? Alias,
    [property: JsonProperty("not_favorited")] bool? NotFavorited,
    [property: JsonProperty("id")] string? Id,
    [property: JsonProperty("has_icon")] string? HasIcon,
    [property: JsonProperty("has_image")] bool? HasImage,
    [property: JsonProperty("img")] string? Img,
    [property: JsonProperty("genres")] string? Genres,
    [property: JsonProperty("channels")] string? Channels
);
