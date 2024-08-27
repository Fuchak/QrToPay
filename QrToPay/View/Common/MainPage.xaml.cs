using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Messages;
using QrToPay.ViewModels.Common;

namespace QrToPay.View;
public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        WeakReferenceMessenger.Default.Register<UserLoggedInMessage>(this, async (r, m) => await OnUserLoggedIn());
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await _viewModel.LoadUserDataAsync();
    }

    private async Task OnUserLoggedIn()
    {
        await _viewModel.LoadUserDataAsync();
    }
}