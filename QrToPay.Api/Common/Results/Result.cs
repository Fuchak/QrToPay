namespace QrToPay.Api.Common.Results;

public class Result<T>
{
    public required bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }
    public ErrorType? ErrorType { get; set; }

    public static Result<T> Success(T value) => new() 
    { 
        IsSuccess = true, 
        Value = value, 
        ErrorType = null 
    };

    public static Result<T> Failure(string error, ErrorType errorType) => new() 
    { 
        IsSuccess = false, 
        Error = error, 
        ErrorType = errorType 
    };
}

public enum ErrorType
{
    NotFound,
    NotVerified,
    BadRequest,
    Unauthorized,
    Other
}