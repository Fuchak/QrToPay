using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.SkiSlopes.Slopes;

public class GetSkiSlopesRequestModel : IRequest<Result<IEnumerable<SkiSlopesDto>>>
{
    public required Guid EntityId { get; init; }
}