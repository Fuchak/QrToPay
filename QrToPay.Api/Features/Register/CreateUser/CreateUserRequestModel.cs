using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Register.CreateUser;

public sealed class CreateUserRequestModel : IRequest<Result<CreateUserDto>>
{
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public required string PasswordHash { get; init; }
}