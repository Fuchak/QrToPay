using CommunityToolkit.Maui.Core;

namespace QrToPay.ViewModels;
public partial class ChangeEmailViewModel(IPopupService popupService) : ViewModelBase
{
    private readonly IPopupService popupService = popupService;

    [ObservableProperty]
    private string? newEmail;

    [ObservableProperty]
    private string? password;

    [RelayCommand]
    public async Task DisplayPopup()
    {
        var result = await popupService.ShowPopupAsync<VerificationCodePopupViewModel>();
        if (result is not null)
        {
            // Odbierz kod weryfikacyjny
            var verificationCode = result;
            // Tutaj możesz zrobić coś z kodem weryfikacyjnym
            // np. wysłać go na serwer lub przechowywać lokalnie
            await Shell.Current.DisplayAlert("Kod weryfikacyjny", $"Otrzymany kod: {verificationCode}", "OK");
        }
    }
}