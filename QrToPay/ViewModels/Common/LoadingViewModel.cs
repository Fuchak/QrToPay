namespace QrToPay.ViewModels.Common;

public partial class LoadingViewModel : ViewModelBase
{
    private readonly AuthService _authService;

    public LoadingViewModel(AuthService authService)
    {
        _authService = authService;
    }

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