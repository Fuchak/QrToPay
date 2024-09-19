using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QrToPay.Helpers;

public static class JsonErrorExtractor
{
    /// <summary>
    /// Extracts the error message from an HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>The extracted error message or a default message if not found.</returns>
    public static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            string jsonContent = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(jsonContent);

            if (jsonDoc.RootElement.TryGetProperty("message", out var message))
            {
                return message.GetString() ?? "Nieznany błąd serwera.";
            }

            return "Nieznany błąd serwera.";
        }
        catch
        {
            throw new HttpRequestException(null, null, response.StatusCode);
        }
    }
}