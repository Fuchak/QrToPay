using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.SkiResorts.Prices;

public class GetSkiResortPricesRequestModel : IRequest<Result<IEnumerable<SkiResortPriceDto>>>
{
    public required int SkiResortId { get; init; }
}