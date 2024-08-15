namespace QrToPay.Api.Features.Cities;
public sealed class CitiesDto
{
    public required Guid EntityId { get; init; }
    public required string CityName { get; init; }
}