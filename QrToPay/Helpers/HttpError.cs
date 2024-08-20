using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace QrToPay.Helpers;

public class HttpError
{
    public static string HandleHttpError(HttpRequestException httpEx)
    {
        if (httpEx.StatusCode.HasValue)
        {
            return httpEx.StatusCode.Value switch
            {
                HttpStatusCode.NotFound => "Błąd: Nie znaleziono zasobu. Spróbuj ponownie później.",

                HttpStatusCode.InternalServerError or
                HttpStatusCode.BadGateway or
                HttpStatusCode.ServiceUnavailable or
                HttpStatusCode.GatewayTimeout => "Błąd serwera: Spróbuj ponownie później.",

                HttpStatusCode.BadRequest => "Błąd żądania: Sprawdź dane i spróbuj ponownie.",
                HttpStatusCode.Unauthorized => "Nieautoryzowany dostęp: Zaloguj się ponownie.",
                HttpStatusCode.Forbidden => "Brak uprawnień: Skontaktuj się z administratorem.",
                _ => "Wystąpił problem z połączeniem. Spróbuj ponownie.",
            };
        }

        return "Brak połączenia z internetem.";
    }

    public static string HandleGeneralError()
    {
        return "Wystąpił nieoczekiwany błąd.";
    }
}