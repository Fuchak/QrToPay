using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.SkiSlopes.Prices;

public class GetSkiSlopePricesRequestModel : IRequest<Result<IEnumerable<SkiSlopePriceDto>>>
{
    public required int SkiResortId { get; init; }
}