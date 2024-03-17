using Android.Media.TV;

namespace QrToPay
{
    public partial class LoginPage : ContentPage
    {
    	public LoginPage()
    	{
    		InitializeComponent();
    	}

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }


        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void OnForgotClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ForgotPage");
        }
    }
}