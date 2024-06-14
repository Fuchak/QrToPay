namespace QrToPay.ViewModels;

public partial class LoadingViewModel(AuthService authService) : ViewModelBase
{

    public async Task CheckAuthentication()
    {
        bool isAuthenticated = await authService.IsAuthenticatedAsync();

        if (isAuthenticated)
        {
            await Shell.Current.GoToAsync("///MainPage");
        }
        else
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}