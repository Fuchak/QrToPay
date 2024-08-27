using QRCoder;

namespace QrToPay.Services.Local;

public class QrCodeStorageService
{
    private readonly string _qrCodeDirectory;

    public QrCodeStorageService()
    {
        _qrCodeDirectory = FileSystem.AppDataDirectory;
    }

    // Metoda do generowania i zapisywania kodu QR
    public async Task GenerateAndSaveQrCodeImageAsync(int userId, string token)
    {
        ImageSource imageSource = await Task.Run(() => GenerateQrCodeImage(token));
        byte[] qrCodeBytes = ImageSourceToBytes(imageSource);

        string fileName = GetQrCodeFileName(userId, token);
        string filePath = Path.Combine(_qrCodeDirectory, fileName);

        await File.WriteAllBytesAsync(filePath, qrCodeBytes);
    }

    // Metoda do wczytywania kodu QR z pamięci
    public async Task<ImageSource?> LoadQrCodeImageAsync(int userId, string token)
    {
        string fileName = GetQrCodeFileName(userId, token);
        string filePath = Path.Combine(_qrCodeDirectory, fileName);

        if (File.Exists(filePath))
        {
            byte[] qrCodeBytes = await File.ReadAllBytesAsync(filePath);
            return ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        }

        return null;
    }

    // Metoda do usuwania starych kodów QR
    public void CleanOldQrCodeFiles(TimeSpan maxAge)
    {
        var files = Directory.GetFiles(_qrCodeDirectory, "QRCode_*.png");

        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            if (fileInfo.CreationTimeUtc < DateTime.UtcNow - maxAge)
            {
                File.Delete(file);
            }
        }
    }

    // Pomocnicza metoda do generowania nazwy pliku
    private static string GetQrCodeFileName(int userId, string token)
    {
        return $"QRCode_{userId}_{token}.png";
    }

    // Pomocnicza metoda do generowania obrazu kodu QR
    private static ImageSource GenerateQrCodeImage(string token)
    {
        QRCodeGenerator qRCodeGenerator = new();
        QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(token, QRCodeGenerator.ECCLevel.Q);
        BitmapByteQRCode bitmapByteQRCode = new(qRCodeData);
        byte[] bytes = bitmapByteQRCode.GetGraphic(20);

        return ImageSource.FromStream(() =>
        {
            MemoryStream memoryStream = new(bytes);
            return memoryStream;
        });
    }

    // Pomocnicza metoda do konwersji obrazu na bajty
    private static byte[] ImageSourceToBytes(ImageSource imageSource)
    {
        using MemoryStream memoryStream = new();
        var streamImageSource = (StreamImageSource)imageSource;
        Stream stream = streamImageSource.Stream.Invoke(new CancellationToken()).Result;
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
