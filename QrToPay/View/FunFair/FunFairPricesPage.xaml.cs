using QrToPay.ViewModels.FunFair;

namespace QrToPay.View.FunFair;

public partial class FunFairPricesPage : ContentPage
{
    private readonly FunFairPricesViewModel _viewModel;
    public FunFairPricesPage(FunFairPricesViewModel vm)
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