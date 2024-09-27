using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Extensions;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Cities;

public class GetCitiesHandler : IRequestHandler<GetCitiesRequestModel, Result<IEnumerable<CitiesDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetCitiesHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<CitiesDto>>> Handle(GetCitiesRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            IEnumerable<CitiesDto> response = await _context.ServiceCategories
                            .Where(e => !e.IsDeleted && e.ServiceType == request.ServiceType.ToInt())
                            .Select(e => new CitiesDto
                            {
                                CityName = e.CityName,
                                ServiceId = e.ServiceId
                            })
                            .Distinct()
                            .OrderBy(c => c.CityName)
                            .ToListAsync(cancellationToken);

            if (!response.Any())
            {
                return Result<IEnumerable<CitiesDto>>.Failure($"Nie znaleziono miast.", ErrorType.NotFound);
            }

            return Result<IEnumerable<CitiesDto>>.Success(response.AsEnumerable());
        });
    }
}