using QrToPay.ViewModels.Common;

namespace QrToPay.View;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((LoadingViewModel)BindingContext).CheckAuthentication();
    }
}