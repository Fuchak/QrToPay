namespace QrToPay.Services;

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsSuccess => ErrorMessage == null;

    public static ServiceResult<T> Success(T data) => new() { Data = data };

    public static ServiceResult<T> Failure(string errorMessage) => new() { ErrorMessage = errorMessage };
}