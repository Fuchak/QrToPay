using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Requests;
using QrToPay.Models.Responses;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.ResetPassword;
public partial class ResetPasswordConfirmViewModel : ViewModelBase
{
    private readonly UserService _userService;

    public ResetPasswordConfirmViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string? emailPhone = ResetPasswordViewModel.SharedEmailOrPhone;

    [ObservableProperty]
    private string? code;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [RelayCommand]
    private async Task Confirm()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordConfirm))
            {
                ErrorMessage = "Wszystkie pola są wymagane.";
                return;
            }

            if (Password != PasswordConfirm)
            {
                ErrorMessage = "Hasła nie są zgodne.";
                return;
            }

            if (Password.Length < 8 || !Password.Any(char.IsDigit) || !Password.Any(char.IsUpper) || !Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ErrorMessage = "Hasło nie spełnia wymagań.";
                return;
            }

            ResetPasswordConfirmRequest request = new()
            {
                EmailOrPhone = EmailPhone!,
                VerificationCode = Code,
                NewPassword = Password
            };

            var result = await _userService.ResetPasswordAsync(request);

            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zaktualizowane.", "OK");
                await NavigateAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}