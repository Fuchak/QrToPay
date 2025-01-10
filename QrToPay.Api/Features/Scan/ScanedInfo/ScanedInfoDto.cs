namespace QrToPay.Api.Features.Scan.ScanedInfo;

public sealed class ScanedInfoDto
{
    public required string ServiceName { get; init; }
    public required string AttractionName { get; init; }
    public required int ServiceId { get; init; }
    public required double Price { get; init; }
}