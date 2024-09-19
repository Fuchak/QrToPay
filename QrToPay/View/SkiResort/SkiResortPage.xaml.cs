using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;
public partial class SkiResortPage : ContentPage
{
    private readonly SkiResortViewModel _viewModel;
    public SkiResortPage(SkiResortViewModel vm)
	{
		InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSkiResortsAsync();
    }
}