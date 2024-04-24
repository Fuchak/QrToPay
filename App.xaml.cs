﻿using QrToPay.Services;
using QrToPay.View;

namespace QrToPay
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Routing.RegisterRoute("LoadingPage", typeof(LoadingPage));
            Routing.RegisterRoute("CreateUserPage", typeof(CreateUserPage));
            Routing.RegisterRoute("CreateUserConfirmPage", typeof(CreateUserConfirmPage));
            Routing.RegisterRoute("ResetPasswordPage", typeof(ResetPasswordPage));
            Routing.RegisterRoute("ResetPasswordConfirmPage", typeof(ResetPasswordConfirmPage));
            Routing.RegisterRoute("SkiPage", typeof(SkiPage));
            Routing.RegisterRoute("AccountPage", typeof(AccountPage));
        }
    }
}
