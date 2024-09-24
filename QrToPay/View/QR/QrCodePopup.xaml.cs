using CommunityToolkit.Maui.Views;
using QrToPay.Services.Api;
using QrToPay.ViewModels.QR;

namespace QrToPay.View;
public partial class QrCodePopup : Popup
{
    public QrCodePopup(ImageSource qrCodeImage, QrCodeService qrCodeService, string userUuid, string token, int? remainingTime = null)
    {
        InitializeComponent();
        BindingContext = new QrCodePopupViewModel(qrCodeImage, qrCodeService, userUuid, token, remainingTime);
    }
}