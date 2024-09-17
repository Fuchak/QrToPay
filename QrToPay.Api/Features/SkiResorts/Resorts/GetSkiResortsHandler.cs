using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Features.SkiResorts.Prices;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.SkiResorts.Resorts;

public class GetSkiResortsHandler : IRequestHandler<GetSkiResortsRequestModel, Result<IEnumerable<SkiResortsDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetSkiResortsHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<SkiResortsDto>>> Handle(GetSkiResortsRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            List<SkiResortsDto> response = await _context.SkiResorts
                .Where(s => !s.IsDeleted &&
                            _context.ServiceCategories.Any(e => e.CityName == request.CityName && e.ServiceId == s.ServiceId && !e.IsDeleted))
                .Select(s => new SkiResortsDto
                {
                    SkiResortId = s.SkiResortId,
                    ResortName = _context.ServiceCategories
                        .Where(e => e.ServiceId == s.ServiceId && e.CityName == request.CityName && !e.IsDeleted)
                        .Select(e => e.ServiceName)
                        .FirstOrDefault()!,
                    CityName = request.CityName
                })
                .OrderBy(s => s.ResortName)
                .ToListAsync(cancellationToken);

            if (response.Count == 0)
            {
                return Result<IEnumerable<SkiResortsDto>>.Failure("Nie znaleziono stoków narciarskich w tym mieście.", ErrorType.NotFound);
            }

            return Result<IEnumerable<SkiResortsDto>>.Success(response.AsEnumerable());
        });
    }
}