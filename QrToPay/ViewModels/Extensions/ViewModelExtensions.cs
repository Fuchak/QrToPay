using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.ViewModels.Extensions;
public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ShellViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoadingViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<CreateUserViewModel>();
        builder.Services.AddTransient<ResetPasswordViewModel>();
        builder.Services.AddTransient<ResetPasswordConfirmViewModel>();
        builder.Services.AddTransient<SkiResortViewModel>();
        builder.Services.AddTransient<FunFairViewModel>();
        builder.Services.AddTransient<AccountViewModel>();
        builder.Services.AddTransient<HelpViewModel>();
        builder.Services.AddTransient<TopUpAccountViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<ChangePasswordViewModel>();
        builder.Services.AddTransient<ChangePhoneViewModel>();
        builder.Services.AddTransient<ChangeEmailViewModel>();
        builder.Services.AddTransient<HistoryViewModel>();
        builder.Services.AddTransient<ScanQrCodeViewModel>();
        builder.Services.AddTransient<SkiResortPricesViewModel>();
        builder.Services.AddTransient<SkiResortBuyViewModel>();
        builder.Services.AddTransient<ActiveBiletsViewModel>();
        builder.Services.AddTransient<SkiResortCityViewModel>();
        builder.Services.AddTransient<FunFairCityViewModel>();
        builder.Services.AddTransient<FunFairPricesViewModel>();
        builder.Services.AddTransient<FunFairBuyViewModel>();

        return builder;
    }
}