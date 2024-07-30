using CommunityToolkit.Maui.Views;
using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;

public partial class QrCodePopup : Popup
{
    public QrCodePopup(ImageSource qrCodeImage)
    {
        InitializeComponent();
        BindingContext = new QrCodePopupViewModel(qrCodeImage);
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
}