using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

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