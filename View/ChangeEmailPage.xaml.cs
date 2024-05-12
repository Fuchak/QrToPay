namespace QrToPay.View;

public partial class ChangeEmailPage : ContentPage
{
	public ChangeEmailPage(ChangeEmailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}