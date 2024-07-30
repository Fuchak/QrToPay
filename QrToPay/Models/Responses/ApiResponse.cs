namespace QrToPay.Models.Responses;

public sealed class ApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }

    public ApiResponse() { }

    public ApiResponse(bool success, string? message = null)
    {
        Success = success;
        Message = message;
    }

    public static ApiResponse SuccessResponse(string? message = null) => new (true, message);
    public static ApiResponse ErrorResponse(string? message = null) => new (false, message);
}