using BarcodeScanning;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;

namespace QrToPay
{
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
                .UseBarcodeScanning()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #if DEBUG
    		    builder.Logging.AddDebug();
            #endif

            #if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            {
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
            });
            #endif

            // Rejestrujesz IHttpClientFactory i konfigurujesz klienta HTTP
            builder.Services.AddHttpClient("ApiHttpClient", client =>
            {
                client.BaseAddress = new Uri("https://jzf085m4-5015.euw.devtunnels.ms/");
            });


            builder.Services.AddTransient<AuthService>();
            builder.Services.AddTransientPopup<VerificationCodePopup, VerificationCodePopupViewModel>();
            builder.Services.AddSingleton<QrCodeService>();


            return builder.Build();
        }
    }
}
