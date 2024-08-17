using QrToPay.ViewModels.FunFair;
using QrToPay.Models.Enums;

namespace QrToPay.View;
public partial class FunFairCityPage : ContentPage
{
    private readonly FunFairCityViewModel _viewModel;
    public FunFairCityPage(FunFairCityViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadCitiesAsync((int)EntityCategory.FunFair);
    }
}