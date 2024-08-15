using QrToPay.ViewModels.Common;

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
        if (Current.CurrentPage is MainPage || Current.CurrentPage is LoginPage)
        {
            return false;
        }
        else { 
            GoBack();
            return true;
        }
    }

    private static async void GoBack()
    {
        // Wykonaj niestandardową nawigację wstecz bez animacji
        await Current.GoToAsync("..", false);
    }
}