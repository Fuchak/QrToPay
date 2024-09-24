namespace QrToPay.Models.Requests;

public sealed class ChangePasswordRequest
{
    public string? OldPassword { get; init; }
    public string? NewPassword { get; init; }
    public string? ConfirmNewPassword { get; init; }
}