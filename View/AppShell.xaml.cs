using Microsoft.Maui.Controls.PlatformConfiguration;
using System.ComponentModel.Design;

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
        if (Shell.Current.CurrentPage is MainPage)
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