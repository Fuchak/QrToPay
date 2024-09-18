using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;
public partial class SkiResortBuyPage : ContentPage
{
    private readonly SkiResortBuyViewModel _viewModel;

    public SkiResortBuyPage(SkiResortBuyViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private void OnIncreaseButtonPressed(object sender, EventArgs e)
    {
        _viewModel?.StartRepeatingAction(1);
    }

    private void OnDecreaseButtonPressed(object sender, EventArgs e)
    {
        _viewModel?.StartRepeatingAction(-1);
    }

    private void OnButtonReleased(object sender, EventArgs e)
    {
        _viewModel?.StopRepeatingAction();
    }
}