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
        await _viewModel.LoadCitiesAsync((int)EntityCategory.SkiResort);
    }
}