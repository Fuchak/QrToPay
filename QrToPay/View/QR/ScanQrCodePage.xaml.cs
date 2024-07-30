using BarcodeScanning;
using Microsoft.Maui.ApplicationModel;
using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;

public partial class ScanQrCodePage : ContentPage
{
    public ScanQrCodePage(ScanQrCodeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var viewModel = BindingContext as ScanQrCodeViewModel;
        if (viewModel == null)
            return;

        if (!await viewModel.CheckPermissions())
        {
            bool goToSettings = await DisplayAlert("Błąd",
                    "Nie przyznano wszystkich uprawnień. Niektóre funkcje systemu nie będą dostępne.",
                    "Ustawienia",
                    "OK");

            if (goToSettings)
            {
                AppInfo.Current.ShowSettingsUI();
            }
            return;
        }

        BarcodeScanner.CameraEnabled = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BarcodeScanner.CameraEnabled = false;
    }

}