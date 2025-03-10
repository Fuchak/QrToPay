using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using QrToPay.ViewModels.Authentication;

namespace QrToPay.View;
public partial class VerificationCodePopup : Popup
{
    public VerificationCodePopup(VerificationCodePopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.ClosePopupAction = ClosePopup;
    }

    private async void ClosePopup(string? result)
    {
        await CloseAsync(result);
    }
}