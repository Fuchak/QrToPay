using CommunityToolkit.Maui.Views;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;

namespace QrToPay.View;
public partial class QrCodePopup : Popup
{
    public QrCodePopup(ImageSource qrCodeImage, QrCodeService qrCodeService, int userId, string token, int? remainingTime = null)
    {
        InitializeComponent();
        BindingContext = new QrCodePopupViewModel(qrCodeImage, qrCodeService, userId, token, remainingTime);
    }
}