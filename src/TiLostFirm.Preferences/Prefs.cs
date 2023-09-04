namespace TiLostFirm.Preferences;

public static class Prefs
{
    public const string BaseUrl = "https://www.lostfilm.today";
    public static string DataBaseName => Path.Combine(Environment.CurrentDirectory, "TiLostFilm.DB");
}