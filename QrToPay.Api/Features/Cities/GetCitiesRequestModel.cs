using MediatR;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Cities;

public class GetCitiesRequestModel : IRequest<Result<IEnumerable<CitiesDto>>>
{
    public required ServiceType ServiceType { get; init; }
}