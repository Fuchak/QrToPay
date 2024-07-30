using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;

public partial class ActiveBiletsPage : ContentPage
{
    private readonly ActiveBiletsViewModel _viewModel;
    public ActiveBiletsPage(ActiveBiletsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadActiveTicketsAsync();
    }
}