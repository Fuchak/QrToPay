using AndroidX.Lifecycle;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;
public partial class FunFairPage : ContentPage
{
    private readonly FunFairViewModel _viewModel;
    public FunFairPage(FunFairViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadFunFairsAsync();
    }
}