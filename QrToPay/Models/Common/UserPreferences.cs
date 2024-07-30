namespace QrToPay.Models.Common;
//Saves data about logged user in preferences
public sealed class UserPreferences
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}