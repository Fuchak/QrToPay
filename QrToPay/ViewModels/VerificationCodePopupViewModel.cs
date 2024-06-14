namespace QrToPay.ViewModels;

public partial class VerificationCodePopupViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? verificationCode;
}
