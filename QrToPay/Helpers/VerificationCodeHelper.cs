using QrToPay.ViewModels.Authentication;
using CommunityToolkit.Maui.Views;

namespace QrToPay.Helpers;
public class VerificationCodeHelper
{
    public static async Task<bool> VerifyCodeAsync(string? actualCode)
    {
        VerificationCodePopupViewModel popupViewModel = new ();

        object? result = await Shell.Current.ShowPopupAsync(new VerificationCodePopup(popupViewModel));

        if (result is not null)
        {
            string? enteredCode = result.ToString();
            return string.Equals(enteredCode, actualCode, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}