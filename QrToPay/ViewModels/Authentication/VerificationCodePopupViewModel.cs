namespace QrToPay.ViewModels.Authentication;

public partial class VerificationCodePopupViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? verificationCode;
}