using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.MusicStore.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.MusicStore.ViewModels;

public partial class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;
    
    public AlbumViewModel(Album album)
    {
        _album = album;
    }
    
    public AlbumViewModel() : this(new Album(
        "Unknown Artist", 
        "Unknown Title",
        "https://images.prismic.io/milanote/df7eeb83a07162b45ac2e882cac055de9411054a_cover.jpg?auto=compress,format"))
    {
    }

    public string Artist => _album.Artist;
    public string Title => _album.Title;
    [ObservableProperty] public partial Bitmap? Cover { get; private set; }

    public async Task LoadCover()
    {
        await using (var imageStream = await _album.LoadCoverBitmapAsync())
        {
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
    }
}