namespace QrToPay.Api.Requests.Settings;

public sealed class ChangePasswordRequestModel
{
    public required int UserId { get; init; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmNewPassword { get; init; }
}