using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Common.Results;

public static class ResultHandler
{
    public static async Task<Result<T>> HandleRequestAsync<T>(Func<Task<Result<T>>> request)
    {
        try
        {
            return await request();
        }
        catch (Exception ex)
        {
            // Jeśli wiadomość wyjątku jest pusta lub null, zwracamy domyślny komunikat o błędzie
            string errorMessage = string.IsNullOrEmpty(ex.Message) 
                ? "Wewnętrzny błąd serwera" 
                : ex.Message;

            return Result<T>.Failure(errorMessage, ErrorType.Other);
        }
    }
}