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
        await Task.Run(async () =>
        {
            Email = await GetSecureStorageValueOrDefaultAsync("UserEmail", "brak");
            PhoneNumber = await GetSecureStorageValueOrDefaultAsync("UserPhone", "brak");
        });

        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(PhoneNumber));
    }

    public static async Task<string> GetSecureStorageValueOrDefaultAsync(string key, string defaultValue)
    {
        string? value = await SecureStorage.GetAsync(key);
        return string.IsNullOrEmpty(value) ? defaultValue : value;
    }
}