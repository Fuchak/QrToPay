namespace QrToPay.ViewModels;

public partial class LoadingViewModel(AuthService authService) : ViewModelBase
{
    private readonly AuthService _authService = authService;

    public async Task CheckAuthentication()
    {
        bool isAuthenticated = await _authService.IsAuthenticatedAsync();

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