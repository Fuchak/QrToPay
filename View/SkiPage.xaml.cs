namespace QrToPay.View;

public partial class SkiPage : ContentPage
{
	public SkiPage(SkiViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}