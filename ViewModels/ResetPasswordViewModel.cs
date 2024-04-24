namespace QrToPay.ViewModels;

public partial class ResetPasswordViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? emailPhone;

    [RelayCommand]
    private async Task Confirm()
    {
        EmailPhone = string.Empty;
        await Shell.Current.GoToAsync("ResetPasswordConfirmPage");
    }
}