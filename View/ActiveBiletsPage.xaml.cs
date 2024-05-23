namespace QrToPay.View;

public partial class ActiveBiletsPage : ContentPage
{
    public ActiveBiletsPage(ActiveBiletsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}