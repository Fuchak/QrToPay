namespace QrToPay.ViewModels;
public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ShellViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoadingViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<CreateUserViewModel>();
        builder.Services.AddTransient<CreateUserConfirmViewModel>();
        builder.Services.AddTransient<ResetPasswordViewModel>();
        builder.Services.AddTransient<ResetPasswordConfirmViewModel>();
        builder.Services.AddTransient<SkiViewModel>();
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
        builder.Services.AddTransient<SkiResortViewModel>();
        builder.Services.AddTransient<SkiResortBuyViewModel>();
        builder.Services.AddTransient<ActiveBiletsViewModel>();

        
        return builder;
    }
}
