namespace QrToPay.View;

public partial class SkiResortBuyPage : ContentPage
{
	public SkiResortBuyPage(SkiResortBuyViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}