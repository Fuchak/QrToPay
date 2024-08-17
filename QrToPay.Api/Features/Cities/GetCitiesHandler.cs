using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Cities
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesRequestModel, Result<IEnumerable<CitiesDto>>>
    {
        private readonly QrToPayDbContext _context;

        public GetCitiesHandler(QrToPayDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<CitiesDto>>> Handle(GetCitiesRequestModel request, CancellationToken cancellationToken)
        {
            var cities = await _context.ServiceCategories
                .Where(e => !e.IsDeleted && e.ServiceType == (int)request.ServiceType)
                .Select(e => new CitiesDto
                {
                    CityName = e.CityName,
                    ServiceId = e.ServiceId
                })
                .Distinct()
                .ToListAsync(cancellationToken);

            if (cities.Count == 0)
            {
                return Result<IEnumerable<CitiesDto>>.Failure($"Nie znaleziono miast dla, {request.ServiceType}.");
            }

            return Result<IEnumerable<CitiesDto>>.Success(cities);
        }
    }
}