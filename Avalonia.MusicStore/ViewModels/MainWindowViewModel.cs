using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.MusicStore.Messages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Avalonia.MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<AlbumViewModel> Albums { get; } = new();
    
    [RelayCommand]
    private async Task AddAlbumAsync()
    {
        var album = await WeakReferenceMessenger.Default.Send(new PurchaseAlbumMessage());
        if (album is not null)
        {
            Albums.Add(album);
        }
    }
}