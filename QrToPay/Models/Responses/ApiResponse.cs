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

    public static ApiResponse SuccessResponse(string? message = null) => new(true, message);
    public static ApiResponse ErrorResponse(string? message = null) => new(false, message);
}

//TODO może lepsze ale to potem
/*namespace QrToPay.Models.Responses
{
    public sealed class ApiResponse<T>
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }

        public ApiResponse() { }

        public ApiResponse(bool success, T? data = default, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null) => new(true, data, message);
        public static ApiResponse<T> ErrorResponse(string? message = null) => new(false, default, message);
    }
}*/