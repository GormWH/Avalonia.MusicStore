using System.Threading.Tasks;
using Avalonia.MusicStore.Messages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Avalonia.MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [RelayCommand]
    private async Task AddAlbumAsync()
    {
        var album = await WeakReferenceMessenger.Default.Send(new PurchaseAlbumMessage());
    }
}