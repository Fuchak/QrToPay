using Java.Lang;
using QrToPay.ViewModels.Common;
using QrToPay.ViewModels.Settings;

namespace QrToPay.View;
public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage(ChangePasswordViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}