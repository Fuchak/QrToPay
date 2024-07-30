using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryViewModel _viewModel;
    public HistoryPage(HistoryViewModel vm)
	{
		InitializeComponent();
		BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadHistoryAsync();
    }
}