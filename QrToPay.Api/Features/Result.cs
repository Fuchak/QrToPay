namespace QrToPay.Api.Features;

public class Result<T>
{
    public required bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
}