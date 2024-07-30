namespace QrToPay.ViewModels.QR;
public partial class QrCodePopupViewModel : ViewModelBase
{
    [ObservableProperty]
    private ImageSource qrCodeImage;

    public QrCodePopupViewModel(ImageSource qrCodeImage)
    {
        QrCodeImage = qrCodeImage;
    }
}