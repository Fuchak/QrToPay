using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Support.HelpFormFeature;

public sealed class HelpFormRequestModel : IRequest<Result<string>>
{
    public required string UserName { get; init; }
    public required string UserEmail { get; init; }
    public required string Subject { get; init; }
    public required string Description { get; init; }
    public required string Status { get; init; } // Domyślnie "Nowe"
}