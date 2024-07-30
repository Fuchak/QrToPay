using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;
public partial class CityPage : ContentPage
{
    private readonly CityViewModel _viewModel;
    public string? AttractionType { get; set; }
    public CityPage(CityViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        _viewModel.AttractionType = AttractionType;
        await _viewModel.LoadCitiesAsync();
    }
}