using BarcodeScanning;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;
using Plugin.LocalNotification;
using QrToPay.ViewModels.Extensions;
using QrToPay.View.Extensions;
using QrToPay.Models.Common;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.QR;
using QrToPay.Services.Api;
using QrToPay.Services.Local;
using Android.Graphics.Drawables;
using Microsoft.Maui.Controls.Platform;

namespace QrToPay;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigurePages()
            .ConfigureViewModels()
            .UseMauiCommunityToolkit()
            .UseLocalNotification()
            .UseBarcodeScanning()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });

        #if DEBUG
		    builder.Logging.AddDebug();
        #endif

        #if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, _) =>
        {
            h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
        });
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("NoUnderline", (h, _) =>
        {
            h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
        });
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderline", (h, _) =>
        {
            h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
        });
        #endif

        // Rejestrujesz IHttpClientFactory i konfigurujesz klienta HTTP
        builder.Services.AddHttpClient("ApiHttpClient", client =>
        {
            client.BaseAddress = new Uri("https://pet-optimum-goshawk.ngrok-free.app/");
        });

        builder.Services.AddTransient<AuthService>();

        builder.Services.AddSingleton<VerificationCodeHelper>();
        builder.Services.AddSingleton<QrCodeStorageService>();

        builder.Services.AddTransientPopup<VerificationCodePopup, VerificationCodePopupViewModel>();
        builder.Services.AddTransientPopup<QrCodePopup, QrCodePopupViewModel>();
        builder.Services.AddSingleton<QrCodeService>();

        builder.Services.AddScoped<BalanceService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<TicketService>();
        builder.Services.AddScoped<HelpService>();
        builder.Services.AddScoped<CityService>();
        builder.Services.AddScoped<ScanService>();
        builder.Services.AddScoped<SkiResortService>();
        builder.Services.AddScoped<FunFairService>();

        //usługa lokalnego magazynu by nie przekazywać po ekranach wszystkich danych
        builder.Services.AddSingleton<AppState>();

        return builder.Build();
    }
}