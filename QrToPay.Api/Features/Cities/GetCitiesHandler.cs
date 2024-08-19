using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
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
            List<CitiesDto> response = await _context.ServiceCategories
                            .Where(e => !e.IsDeleted && e.ServiceType == (int)request.ServiceType)
                            .Select(e => new CitiesDto
                            {
                                CityName = e.CityName,
                                ServiceId = e.ServiceId
                            })
                            .Distinct()
                            .ToListAsync(cancellationToken);

            if (response.Count == 0)
            {
                throw new Exception($"Nie znaleziono miast.");
            }

            return response.AsEnumerable();
        });
    }
}