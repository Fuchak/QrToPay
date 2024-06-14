using QRCoder;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;


namespace QrToPay.Services;
public class QrCodeService
{
    public Task<string> GenerateQrCodeAsync(string resortName, string city, string biletType, string price, string points)
    {
        var ticketInfo = new
        {
            ResortName = resortName,
            City = city,
            BiletType = biletType,
            Price = price,
            Points = points
        };

        var json = JsonSerializer.Serialize(ticketInfo);
        var fileName = Path.Combine(FileSystem.CacheDirectory, "qrcode.png");
        var jsonFileName = Path.ChangeExtension(fileName, ".json");

        using QRCodeGenerator qrGenerator = new();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
        using PngByteQRCode qrCode = new(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);

        File.WriteAllBytes(fileName, qrCodeImage);
        File.WriteAllText(jsonFileName, json);
        Preferences.Set("QrCodePath", fileName);
        return Task.FromResult(fileName);
    }
}