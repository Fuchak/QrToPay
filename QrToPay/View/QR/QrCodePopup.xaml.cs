using CommunityToolkit.Maui.Views;
using QrToPay.ViewModels.QR;

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