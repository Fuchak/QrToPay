namespace QrToPay.Api.Common.Services;

public interface ICurrentUserService
{
    int UserId { get; }
    string? Email { get; }
    string? PhoneNumber { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
       _httpContextAccessor.HttpContext?.Items["UserId"] as int?
       ?? throw new InvalidOperationException("UserId nie jest dostępny w kontekście HTTP.");

    public string? Email =>
        _httpContextAccessor.HttpContext?.Items["UserEmail"] as string
        ?? throw new InvalidOperationException("Email nie jest dostępny w kontekście HTTP.");

    public string? PhoneNumber =>
        _httpContextAccessor.HttpContext?.Items["UserPhoneNumber"] as string
        ?? throw new InvalidOperationException("Numer telefonu nie jest dostępny w kontekście HTTP.");
}
