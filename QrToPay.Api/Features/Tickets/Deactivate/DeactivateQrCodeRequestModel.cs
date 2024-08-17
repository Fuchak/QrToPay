using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Deactivate;

public class DeactivateQrCodeRequestModel : IRequest<Result<bool>>
{
    public required Guid Token { get; init; }
    public required int UserID { get; init; }
}