using CommunityToolkit.Maui.Views;
using QrToPay.Services.Api;
using QrToPay.Services.Local;
using QrToPay.ViewModels.QR;

namespace QrToPay.View;
public partial class QrCodePopup : Popup
{
    public QrCodePopup(ImageSource qrCodeImage, QrCodeService qrCodeService, CacheService cacheService, string token, int? remainingTime = null)
    {
        InitializeComponent();
        BindingContext = new QrCodePopupViewModel(qrCodeImage, qrCodeService, cacheService, token, remainingTime);
    }
}