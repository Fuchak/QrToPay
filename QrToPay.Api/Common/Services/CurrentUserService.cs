namespace QrToPay.Api.Common.Services;

public class CurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
       _httpContextAccessor.HttpContext?.Items["UserId"] as int?
       ?? throw new InvalidOperationException("UserId nie jest dostępny w kontekście HTTP.");
}