namespace QrToPay.ViewModels.FlyoutMenu;

public partial class AccountViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? phoneNumber;

    [RelayCommand]
    public async Task LoadUserData()
    {
        if(IsLoading) return;
        else
        {
            IsLoading = true;
            Email = await GetSecureStorageValueOrDefaultAsync(SecureStorageConst.UserEmail, "brak");
            PhoneNumber = await GetSecureStorageValueOrDefaultAsync(SecureStorageConst.UserPhone, "brak");
            IsLoading = false;
        }
    }

    public static async Task<string> GetSecureStorageValueOrDefaultAsync(string key, string defaultValue) 
        => await SecureStorage.GetAsync(key) ?? defaultValue;
}