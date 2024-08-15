using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Scan.ScanedInfo;
public sealed class ScanedInfoRequestModel : IRequest<Result<ScanedInfoDto>>
{
    [FromRoute(Name = "qrCode")]
    public required string QrCode { get; init; }
}