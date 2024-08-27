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
using QrToPay.Services.Local;

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
        bool permissionGranted = await PermissionService.RequestPermissionAsync<Permissions.Camera>("kamery");

        if (permissionGranted)
        {
            BarcodeScanner.CameraEnabled = true;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BarcodeScanner.CameraEnabled = false;
    }
}