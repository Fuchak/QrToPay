using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Scan.ScanedInfo
{
    public sealed class ScanedInfoRequestModel : IRequest<Result<ScanedInfoDto>>
    {
        public required string QrCode { get; init; }
    }
}