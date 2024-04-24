namespace QrToPay.View;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
}