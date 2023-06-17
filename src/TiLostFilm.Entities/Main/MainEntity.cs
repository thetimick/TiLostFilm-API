namespace TiLostFilm.Entities.Main;

public record MainEntity(
    List<MainSerial> NewSerials,
    List<MainEpisode> NewEpisodes,
    List<MainSeason> NewSeasons
);

public record MainSerial(
    string PosterUrl,
    string Title,
    string TitleOrig
);

public record MainEpisode(
    string PosterUrl,
    string EpisodeNumber,
    string DateRelease
);

public record MainSeason(
    string PosterUrl,
    string SeasonNumber,
    string DateRelease
);