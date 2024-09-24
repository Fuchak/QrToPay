using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Deactivate;

public class DeactivateQrCodeRequestModel : IRequest<Result<SuccesMessageDto>>
{
    public required Guid Token { get; init; }
}