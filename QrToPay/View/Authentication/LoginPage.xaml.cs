using QrToPay.Services.Local;
using QrToPay.ViewModels.Authentication;

namespace QrToPay.View;
public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        InitializeAsync();
    }
    private async static void InitializeAsync()
    {
        await PermissionService.RequestPermissionAsync<Permissions.PostNotifications>("powiadomieñ");
    }
}