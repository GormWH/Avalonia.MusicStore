using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace Avalonia.MusicStore.Models;

public class Album
{
    private static iTunesSearchManager s_SearchManager = new();
    
    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }

    private static HttpClient s_HttpClient = new();
    private string CachePath => $"./Cache/{SanitizeFileName(Artist)} - {SanitizeFileName(Title)}";

    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }

    public static async Task<IEnumerable<Album>> SearchAsync(string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return [];
        }

        var query = await s_SearchManager.GetAlbumsAsync(searchTerm)
            .ConfigureAwait(false);

        return query.Albums.Select(x =>
            new Album(x.ArtistName, x.CollectionName,
                x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }

    public async Task<Stream> LoadCoverBitmapAsync()
    {
        if (File.Exists(CachePath + ".bmp"))
        {
            return File.OpenRead(CachePath + ".bmp");
        }
        else
        {
            var data = await s_HttpClient.GetByteArrayAsync(CoverUrl);
            return new MemoryStream(data);
        }
    }

    #region Save to disk
    public async Task SaveAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        using (var fs = File.OpenWrite(CachePath))
        {
            await SaveToStreamAsync(this, fs);
        }
    }

    public Stream SaveCoverBitmapStream()
    {
        return File.OpenWrite(CachePath + ".bmp");
    }

    private static async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }
    #endregion
    
    #region load from disk
    public static async Task<Album> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }

    public static async Task<IEnumerable<Album>> LoadCachedAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        var results = new List<Album>();

        foreach (var file in Directory.EnumerateFiles("./Cache"))
        {
            if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

            await using var fs = File.OpenRead(file);
            results.Add(await Album.LoadFromStream(fs).ConfigureAwait(false));
        }

        return results;
    }
    #endregion

    private static string SanitizeFileName(string fileName)
    {
        var sb = new StringBuilder();
        var invalidChars = Path.GetInvalidFileNameChars();
        
        foreach (var c in fileName)
        {
            sb.Append(invalidChars.Contains(c) ? '_' : c); // Replace invalid characters with underscore
        }

        return sb.ToString();
    }
}