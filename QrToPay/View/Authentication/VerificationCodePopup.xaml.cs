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

public partial class VerificationCodePopup : Popup
{
    public VerificationCodePopup(VerificationCodePopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    async void OnConfirmButtonClicked(object? sender, EventArgs e)
    {
        var viewModel = (VerificationCodePopupViewModel)BindingContext;
        await CloseAsync(viewModel.VerificationCode);
    }
}