using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.Services.Extensions;

public static class ServiceExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        //API
        builder.Services.AddTransient<AuthService>();
        builder.Services.AddScoped<BalanceService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<TicketService>();
        builder.Services.AddScoped<HelpService>();
        builder.Services.AddScoped<CityService>();
        builder.Services.AddScoped<ScanService>();
        builder.Services.AddScoped<SkiResortService>();
        builder.Services.AddScoped<FunFairService>();
        builder.Services.AddSingleton<QrCodeService>();
        builder.Services.AddSingleton<CacheService>();

        //LOCAL
        builder.Services.AddSingleton<QrCodeStorageService>();

        return builder;
    }
}