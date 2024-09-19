using QrToPay.ViewModels.FunFair;

namespace QrToPay.View.FunFair;
public partial class FunFairBuyPage : ContentPage
{
    private readonly FunFairBuyViewModel _viewModel;

    public FunFairBuyPage(FunFairBuyViewModel vm)
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