using Microsoft.Maui.Controls.PlatformConfiguration;
using System.ComponentModel.Design;
using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.FunFair;
using QrToPay.ViewModels.Authentication;
using QrToPay.ViewModels.FlyoutMenu;
using QrToPay.ViewModels.QR;
using QrToPay.ViewModels.ResetPassword;
using QrToPay.ViewModels.Settings;
using QrToPay.ViewModels.SkiResort;

namespace QrToPay.View;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = new ShellViewModel();
    }

    protected override bool OnBackButtonPressed()
    {
        if (Shell.Current.CurrentPage is MainPage || Shell.Current.CurrentPage is LoginPage)
        {
            return false;
        }
        else { 
            GoBack();
            return true;
        }
    }

    private async void GoBack()
    {
        // Wykonaj niestandardową nawigację wstecz bez animacji
        await Shell.Current.GoToAsync("..", false);
    }
}