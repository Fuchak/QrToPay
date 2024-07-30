using System.Diagnostics;
using System.Threading.Tasks;
using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;

public partial class SkiPage : ContentPage
{
    private readonly SkiViewModel _viewModel;
    public SkiPage(SkiViewModel vm)
	{
		InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadSkiResortsAsync();
    }
}