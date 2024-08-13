using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Auth.Login;

public sealed class LoginRequestModel : IRequest<Result<LoginDto>>
{
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public required string PasswordHash { get; init; }
}