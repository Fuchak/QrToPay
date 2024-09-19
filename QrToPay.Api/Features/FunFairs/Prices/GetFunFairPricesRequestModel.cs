using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.FunFairs.Prices;

public class GetFunFairPricesRequestModel : IRequest<Result<IEnumerable<FunFairPriceDto>>>
{
    public required int FunFairId { get; init; }
}