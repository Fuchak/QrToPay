using QrToPay.ViewModels.FlyoutMenu;

namespace QrToPay.View;
public partial class ActiveBiletsPage : ContentPage
{
    private readonly ActiveBiletsViewModel _viewModel;
    public ActiveBiletsPage(ActiveBiletsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (Shell.Current.FlyoutIsPresented)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Task.Delay(250);
        }
        await _viewModel.LoadActiveTicketsAsync();
    }
}