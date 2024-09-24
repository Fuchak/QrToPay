namespace QrToPay.ViewModels.FlyoutMenu;

public partial class AccountViewModel : ViewModelBase
{

    //To NIBY MOŻNA wziąć gdzies odczytać raz i se trzymać potem tylko udpaty modelu robić i tyle nie ma co tego co chwilę czytać z tego storage tylko daje to dziwny obraz UI bo taki zlagowany
    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? phoneNumber;

    [RelayCommand]
    public async Task LoadUserData()
    {
        Email = await GetSecureStorageValueOrDefaultAsync(SecureStorageConst.UserEmail, "brak");
        PhoneNumber = await GetSecureStorageValueOrDefaultAsync(SecureStorageConst.UserPhone, "brak");
    }

    public static async Task<string> GetSecureStorageValueOrDefaultAsync(string key, string defaultValue) 
        => await SecureStorage.GetAsync(key) ?? defaultValue;
}