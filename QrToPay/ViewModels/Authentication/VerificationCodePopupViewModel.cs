namespace QrToPay.ViewModels.Authentication;

public partial class VerificationCodePopupViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? verificationCode;

    public Action<string?>? ClosePopupAction { get; set; }

    [RelayCommand]
    private void Confirm()
    {//TODO tutaj isbusy?
        ClosePopupAction?.Invoke(VerificationCode);
    }

    [RelayCommand]
    private void Cancel()
    {
        ClosePopupAction?.Invoke(null);
    }
}