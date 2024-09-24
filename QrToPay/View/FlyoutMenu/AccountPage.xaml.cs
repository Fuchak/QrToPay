using QrToPay.ViewModels.FlyoutMenu;

namespace QrToPay.View;
public partial class AccountPage : ContentPage
{
    private readonly AccountViewModel _viewModel;
    public AccountPage(AccountViewModel vm)
	{
        InitializeComponent();
        BindingContext = _viewModel = vm;

        //vm.LoadUserData();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUserData();
    }
}