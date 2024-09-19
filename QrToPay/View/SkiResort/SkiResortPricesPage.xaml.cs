using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;
public partial class SkiResortPricesPage : ContentPage
{
    private readonly SkiResortPricesViewModel _viewModel;
    public SkiResortPricesPage(SkiResortPricesViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}