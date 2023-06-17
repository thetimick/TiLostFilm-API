using TiLostFirm.Parser.Services;

namespace TiLostFirm.Parser;

public class LostFilmParser
{
    public readonly MainService Main = new();
    public readonly ContentService Content = new();
    public readonly EpisodeService Episode = new();
}