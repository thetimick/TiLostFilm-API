using AngleSharp;
using AngleSharp.Dom;

namespace TiLostFilm.Cache;

internal static class WebService
{
    internal static async Task<string> LoadAsync(string url)
    {
        var context = new BrowsingContext(Configuration.Default.WithDefaultLoader());
        return (await context.OpenAsync(new Url(url))).Source.Text;
    }
}