namespace QrToPay.View;

public partial class CreateUserConfirmPage : ContentPage
{
    public CreateUserConfirmPage(CreateUserConfirmViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}