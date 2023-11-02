namespace TiLostFilm.Entities.Shedule;

public record SheduleEntity(
    SheduleMeta? Meta,
    SheduleData? Data
);

public record SheduleMeta(
    string Url,
    DateTime TimeStamp
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
    string? EpisodeTitle,
    DateOnly? ReleaseDate
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