namespace QrToPay.View;

public partial class HelpPage : ContentPage
{
	public HelpPage(HelpViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}