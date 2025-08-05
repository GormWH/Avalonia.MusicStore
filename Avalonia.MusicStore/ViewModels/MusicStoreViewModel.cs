using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.MusicStore.ViewModels;

public partial class MusicStoreViewModel : ViewModelBase
{
    [ObservableProperty] public partial string? SearchText { get; set; }
    [ObservableProperty] public partial bool IsBusy { get; private set; }
    [ObservableProperty] public partial AlbumViewModel? SelectedAlbum { get; set; }
    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();
    
    public MusicStoreViewModel()
    {
        SearchResults.Add(new AlbumViewModel());
        SearchResults.Add(new AlbumViewModel());
        SearchResults.Add(new AlbumViewModel());
    }
}