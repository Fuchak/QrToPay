using QrToPay.ViewModels.ResetPassword;

namespace QrToPay.View;
public partial class ResetPasswordConfirmPage : ContentPage
{
	public ResetPasswordConfirmPage(ResetPasswordConfirmViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}