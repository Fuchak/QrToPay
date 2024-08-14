using QrToPay.ViewModels.FlyoutMenu;

namespace QrToPay.View;
public partial class AccountPage : ContentPage
{
	public AccountPage(AccountViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
        vm.LoadUserData();
    }
}