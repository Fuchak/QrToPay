namespace QrToPay.View;

public partial class SkiResortPage : ContentPage
{
	public SkiResortPage(SkiResortViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
}