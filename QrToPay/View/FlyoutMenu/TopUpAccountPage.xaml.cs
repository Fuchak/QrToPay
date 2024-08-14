using QrToPay.ViewModels.FlyoutMenu;

namespace QrToPay.View;
public partial class TopUpAccountPage : ContentPage
{
	public TopUpAccountPage(TopUpAccountViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}