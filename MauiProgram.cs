using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
//Nie wiem czemu ale tego jakby nie ma ale bez tego są podkreślenia w EntryHandler
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #if DEBUG
    		    builder.Logging.AddDebug();
            #endif

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Placeholder", (h, v) =>
            {
            #if ANDROID
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            #endif
            });


            // Rejestrujesz IHttpClientFactory i konfigurujesz klienta HTTP
            builder.Services.AddHttpClient("ApiHttpClient", client =>
            {
                client.BaseAddress = new Uri("https://jzf085m4-5015.euw.devtunnels.ms/");
            });


            builder.Services.AddTransient<AuthService>();
            builder.Services.AddTransientPopup<VerificationCodePopup, VerificationCodePopupViewModel>();


            return builder.Build();
        }
    }
}
