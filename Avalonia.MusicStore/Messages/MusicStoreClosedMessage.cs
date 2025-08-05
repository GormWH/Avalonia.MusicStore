using Avalonia.MusicStore.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Avalonia.MusicStore.Messages;

public class MusicStoreClosedMessage(AlbumViewModel selectedAlbum)
{
    public AlbumViewModel SelectedAlbum { get; } = selectedAlbum;
}