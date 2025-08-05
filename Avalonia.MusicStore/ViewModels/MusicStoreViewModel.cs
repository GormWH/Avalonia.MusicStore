using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.MusicStore.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.MusicStore.ViewModels;

public partial class MusicStoreViewModel : ViewModelBase
{
    [ObservableProperty] public partial string? SearchText { get; set; }
    [ObservableProperty] public partial bool IsBusy { get; private set; }
    [ObservableProperty] public partial AlbumViewModel? SelectedAlbum { get; set; }
    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();
    
    private async Task DoSearch(string? term)
    {
        IsBusy = true;
        SearchResults.Clear();

        var albums = await Album.SearchAsync(term);

        foreach (var album in albums)
        {
            var vm = new AlbumViewModel(album);
            SearchResults.Add(vm);
        }

        IsBusy = false;
    }
    
    partial void OnSearchTextChanged(string? value)
    {
        _ = DoSearch(SearchText);
    }
}