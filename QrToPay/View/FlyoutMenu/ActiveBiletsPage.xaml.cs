using QrToPay.ViewModels.FlyoutMenu;

namespace QrToPay.View;
public partial class ActiveBiletsPage : ContentPage
{
    private readonly ActiveBiletsViewModel _viewModel;
    public ActiveBiletsPage(ActiveBiletsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadActiveTicketsAsync();
    }
}