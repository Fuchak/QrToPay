using System.Text.Json;

namespace QrToPay.Helpers;
public static class JsonErrorExtractor
{
    /// <summary>
    /// Extracts the error message from a JSON response.
    /// </summary>
    /// <param name="jsonContent">The JSON content as a string.</param>
    /// <returns>The extracted error message or a default message if not found.</returns>
    public static string ExtractErrorMessage(string jsonContent)
    {
        using var jsonDoc = JsonDocument.Parse(jsonContent);
         if (jsonDoc.RootElement.TryGetProperty("message", out var message))
        {
            return message.GetString() ?? "Nieznany błąd serwera.";
        }

        return "Nieznany błąd serwera.";
    }
}