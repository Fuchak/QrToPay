using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.SkiResorts.Resorts;

public class GetSkiResortsRequestModel : IRequest<Result<IEnumerable<SkiResortsDto>>>
{
    public required Guid ServiceId { get; init; }
}