namespace QrToPay.ViewModels;

public partial class CreateUserConfirmViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? code;

    [RelayCommand]
    private async Task Confirm()
    {
        Code = string.Empty;
        await Shell.Current.GoToAsync("///LoginPage");
    }
}