namespace QrToPay
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(CreateUserPage), typeof(CreateUserPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Routing.RegisterRoute(nameof(ResetPasswordConfirmPage), typeof(ResetPasswordConfirmPage));
            Routing.RegisterRoute(nameof(SkiPage), typeof(SkiPage));
            Routing.RegisterRoute(nameof(FunFairPage), typeof(FunFairPage));
            Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
            Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
            Routing.RegisterRoute(nameof(TopUpAccountPage), typeof(TopUpAccountPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(ChangePhonePage), typeof(ChangePhonePage));
            Routing.RegisterRoute(nameof(ChangeEmailPage), typeof(ChangeEmailPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(ScanQrCodePage), typeof(ScanQrCodePage));
            Routing.RegisterRoute(nameof(SkiResortPage), typeof(SkiResortPage));
            Routing.RegisterRoute(nameof(SkiResortBuyPage), typeof(SkiResortBuyPage));
            Routing.RegisterRoute(nameof(ActiveBiletsPage), typeof(ActiveBiletsPage));
            Routing.RegisterRoute(nameof(SkiResortCityPage), typeof(SkiResortCityPage));
            Routing.RegisterRoute(nameof(FunFairCityPage), typeof(FunFairCityPage));
        }
    }
}
