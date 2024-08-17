using CommunityToolkit.Maui.Views;

namespace QrToPay.ViewModels.QR;
public partial class QrCodePopupViewModel : ViewModelBase
{
    [ObservableProperty]
    private ImageSource qrCodeImage;

    public QrCodePopupViewModel(ImageSource qrCodeImage)
    {
        QrCodeImage = qrCodeImage;
    }

    [RelayCommand]
    public async Task ClosePopupAsync(Popup popup)
    {
        await popup.CloseAsync();
    }
}