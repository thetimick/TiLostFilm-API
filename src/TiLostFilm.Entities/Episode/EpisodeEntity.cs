namespace TiLostFilm.Entities.Episode;

public record EpisodeEntity(
    string SeasonNumber,
    string EpisodeNumber,
    string Title,
    string TitleOrig,
    string PosterUrl,
    double Rating,
    string? DateReleaseRu,
    string? Duration,
    string? RatingIMDb,
    string? Description
);