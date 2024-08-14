using MediatR;
using QrToPay.Api.Common.Dtos;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Cities;

public class GetCitiesRequestModel : IRequest<Result<IEnumerable<CityDto>>>
{
    public required EntityCategory EntityType { get; init; }
}