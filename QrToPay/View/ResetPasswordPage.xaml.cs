namespace QrToPay.View;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage(ResetPasswordViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}