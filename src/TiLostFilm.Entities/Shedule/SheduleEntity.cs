namespace TiLostFilm.Entities.Shedule;

public record SheduleEntity(
    SheduleData? Data
);

public record SheduleData(
    List<SheduleDataItem>? Today,
    List<SheduleDataItem>? Tomorrow,
    List<SheduleDataItem>? OnThisWeek,
    List<SheduleDataItem>? OnNextWeek,
    List<SheduleDataItem>? Future
);

public record SheduleDataItem(
    string? Cover,
    SheduleSerialTitle? Title,
    SheduleEpisodeNumber? EpisodeNumber,
    SheduleEpisodeTitle? EpisodeTitle,
    string? ReleaseDate
);

public record SheduleSerialTitle(
    string? Ru,
    string? En
);

public record SheduleEpisodeNumber(
    int? Season,
    int? Episode
);

public record SheduleEpisodeTitle(
    string? Ru,
    string? En
);