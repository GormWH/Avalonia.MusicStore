using System;
using Avalonia.MusicStore.Models;

namespace Avalonia.MusicStore.ViewModels;

public partial class AlbumViewModel(Album album) : ViewModelBase
{
    public AlbumViewModel() : this(new Album(
        "Unknown Artist", 
        "Unknown Title",
        "https://images.prismic.io/milanote/df7eeb83a07162b45ac2e882cac055de9411054a_cover.jpg?auto=compress,format"))
    {
    }

    public string Artist => album.Artist;
    public string Title => album.Title;
}