namespace QrToPay.Api.Common.Helpers;
public abstract class AuthenticationHelper
{
    public static string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
    public static bool VerifyPassword(string enteredPassword, string storedHash)
        => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
    public static string GenerateVerificationCode()
        => new Random().Next(100000, 999999).ToString();
}