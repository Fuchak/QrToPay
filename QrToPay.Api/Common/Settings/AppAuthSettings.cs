namespace QrToPay.Api.Common.Settings;

public sealed class AppAuthSettings
{
    public string Secret { get; init; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}