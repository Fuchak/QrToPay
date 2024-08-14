using QrToPay.ViewModels.Settings;

namespace QrToPay.View;
public partial class ChangePhonePage : ContentPage
{
    public ChangePhonePage(ChangePhoneViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}