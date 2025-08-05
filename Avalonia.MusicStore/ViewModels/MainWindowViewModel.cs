﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.MusicStore.Messages;
using Avalonia.MusicStore.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Avalonia.MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<AlbumViewModel> Albums { get; } = new();
    
    public MainWindowViewModel()
    {
        LoadAlbums();
    }
    
    [RelayCommand]
    private async Task AddAlbumAsync()
    {
        var album = await WeakReferenceMessenger.Default.Send(new PurchaseAlbumMessage());
        if (album is not null)
        {
            Albums.Add(album);
            await album.SaveToDiskAsync(); // Add this line
        }
    }
    
    private async void LoadAlbums()
    {
        var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x)).ToList();
        foreach (var album in albums)
        {
            Albums.Add(album);
        }
        var coverTasks = albums.Select(album => album.LoadCover());
        await Task.WhenAll(coverTasks);
    }

}