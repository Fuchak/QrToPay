namespace QrToPay.Api.Responses;
public sealed class CityDto
{
    public required Guid EntityId { get; init; }
    public required string CityName { get; init; }
}