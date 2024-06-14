using BarcodeScanning;
using Microsoft.Maui.ApplicationModel;

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
            bool goToSettings = await DisplayAlert("B³¹d",
                    "Nie przyznano wszystkich uprawnieñ. Niektóre funkcje systemu nie bêd¹ dostêpne.",
                    "Ustawienia",
                    "OK");

            if (goToSettings)
            {
                AppInfo.Current.ShowSettingsUI();
            }
            return;
        }

        barcodeScanner.CameraEnabled = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        barcodeScanner.CameraEnabled = false;
    }

}