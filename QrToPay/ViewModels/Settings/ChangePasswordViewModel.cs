using System.Net.Http.Json;
using System.Diagnostics;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.Settings;
public partial class ChangePasswordViewModel : ViewModelBase
{
    private readonly UserService _userService;

    public ChangePasswordViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string? oldPassword;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [RelayCommand]
    private async Task Confirm()
    {
        if(IsBusy) return;
        try
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordConfirm))
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

            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                ErrorMessage = "Nie można znaleźć identyfikatora użytkownika.";
                return;
            }

            ChangePasswordRequest changePasswordRequest = new()
            {
                UserId = userId,
                OldPassword = OldPassword,
                NewPassword = Password,
                ConfirmNewPassword = PasswordConfirm
            };

            var result = await _userService.ChangePasswordAsync(changePasswordRequest);

            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zmienione.", "OK");
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
            OldPassword = string.Empty;
            Password = string.Empty;
            PasswordConfirm = string.Empty;
        }
    }
}
