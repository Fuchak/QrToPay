using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Scan.Purchase;

public sealed class ScanPurchaseRequestModel : IRequest<Result<string>>
{
    public required int UserId { get; init; }
    public required string Type { get; init; }
    public required Guid ServiceId { get; init; }    
    public required decimal Price { get; init; }
}