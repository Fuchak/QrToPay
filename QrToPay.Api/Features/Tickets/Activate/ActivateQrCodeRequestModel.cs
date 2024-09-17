using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Activate;

public class ActivateQrCodeRequestModel : IRequest<Result<SuccesMessageDto>>
{
    public required Guid Token { get; init; }
    public required int UserID { get; init; }
}