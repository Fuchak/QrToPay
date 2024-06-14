namespace QrToPay.ViewModels;

public partial class AccountViewModel : ViewModelBase
{
    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? phoneNumber;

    [RelayCommand]
    public void LoadUserData()
    {
        UserId = Preferences.Get("UserId", 0);
        Email = Preferences.Get("UserEmail", "brak");
        PhoneNumber = Preferences.Get("UserPhone", "brak");
    }
}