﻿using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace QrToPay.Helpers;

public static class HttpError
{
    public static string HandleError(Exception ex)
    {
        if (ex is HttpRequestException httpEx)
        {
            if (httpEx.StatusCode.HasValue)
            {
                return httpEx.StatusCode.Value switch
                {
                    HttpStatusCode.NotFound => "Błąd: Nie znaleziono zasobu. Spróbuj ponownie później.",

                    HttpStatusCode.InternalServerError or
                    HttpStatusCode.BadGateway or
                    HttpStatusCode.ServiceUnavailable or
                    HttpStatusCode.TemporaryRedirect or
                    HttpStatusCode.GatewayTimeout => "Błąd serwera: Spróbuj ponownie później.",

                    HttpStatusCode.BadRequest => "Błąd żądania: Sprawdź dane i spróbuj ponownie.",
                    HttpStatusCode.Unauthorized => "Brak autoryzacji: Zaloguj się ponownie.",
                    HttpStatusCode.Forbidden => "Brak uprawnień: Skontaktuj się z administratorem.",
                    _ => "Wystąpił problem z połączeniem. Spróbuj ponownie.",
                };
            }
            return "Brak połączenia z internetem.";
        }
        return "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
    }
}