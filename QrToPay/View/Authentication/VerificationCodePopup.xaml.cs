using CommunityToolkit.Maui.Views;
using QrToPay.ViewModels.Authentication;

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