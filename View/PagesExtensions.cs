namespace QrToPay.View;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<CreateUserPage>();
            builder.Services.AddTransient<CreateUserConfirmPage>();
            builder.Services.AddTransient<ResetPasswordPage>();
            builder.Services.AddTransient<ResetPasswordConfirmPage>();
            builder.Services.AddTransient<SkiPage>();
            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<HelpPage>();
            builder.Services.AddTransient<TopUpAccountPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<ChangePasswordPage>();
            builder.Services.AddTransient<ChangePhonePage>();
            builder.Services.AddTransient<ChangeEmailPage>();
            builder.Services.AddTransient<HistoryPage>();

            return builder;
        }   
}

