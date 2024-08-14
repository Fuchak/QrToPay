using QrToPay.ViewModels.Authentication;

namespace QrToPay.View;
public partial class CreateUserPage : ContentPage
{
    public CreateUserPage(CreateUserViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}