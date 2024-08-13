using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;
using QrToPay.Models.Enums;

namespace QrToPay.View;
public partial class SkiResortCityPage : ContentPage
{
    private readonly SkiResortCityViewModel _viewModel;
    public SkiResortCityPage(SkiResortCityViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadCitiesAsync();
    }
}