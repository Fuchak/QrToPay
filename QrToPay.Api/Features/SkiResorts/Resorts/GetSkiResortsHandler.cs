using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
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
        List<SkiResortsDto> response = await _context.SkiResorts
                    .Where(s => !s.IsDeleted && s.ServiceId == request.ServiceId)
                    .Select(s => new SkiResortsDto
                    {
                        SkiResortId = s.SkiResortId,
                        ResortName = _context.ServiceCategories
                            .Where(e => e.ServiceId == s.ServiceId && !e.IsDeleted)
                            .Select(e => e.ServiceName)
                            .FirstOrDefault()!,
                        CityName = _context.ServiceCategories
                            .Where(e => e.ServiceId == s.ServiceId && !e.IsDeleted)
                            .Select(e => e.CityName)
                            .FirstOrDefault()!,
                    })
                    .ToListAsync(cancellationToken);

        if (response.Count == 0)
        {
            return Result<IEnumerable<SkiResortsDto>>.Failure($"Nie znalezionio stoków narciarskich w tym mieście");
        }

        return Result<IEnumerable<SkiResortsDto>>.Success(response);
    }
}