using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.FunFairs.FunFairs;

public class GetFunFairsRequestModel : IRequest<Result<IEnumerable<FunFairsDto>>>
{
    public required string CityName { get; init; }
}