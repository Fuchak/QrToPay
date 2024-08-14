using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;
public partial class SkiResortBuyPage : ContentPage
{
    private readonly SkiResortBuyViewModel _viewModel;

    public SkiResortBuyPage(SkiResortBuyViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        await _viewModel.InitializeAsync();
    }
}