using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Scan.Purchase;

public sealed class ScanPurchaseRequestModel : IRequest<Result<SuccesMessageDto>>
{
    public required string ServiceName { get; init; }
    public required Guid ServiceId { get; init; }
    public required decimal Price { get; init; }
}