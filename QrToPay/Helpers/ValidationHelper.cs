using System.Text.RegularExpressions;

namespace QrToPay.Helpers;
public static partial class ValidationHelper
{
    private static readonly Regex EmailRegex = MyRegex1();
    private static readonly Regex PhoneRegex = MyRegex(); 

    public static bool IsEmail(string input)
    {
        return EmailRegex.IsMatch(input);
    }

    public static bool IsPhoneNumber(string input)
    {
        return PhoneRegex.IsMatch(input);
    }

    [GeneratedRegex(@"^\+48\d{9}$")]
    private static partial Regex MyRegex();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex MyRegex1();
}