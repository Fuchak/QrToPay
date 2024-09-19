using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.FunFairs.FunFairs;

public class GetFunFairsHandler : IRequestHandler<GetFunFairsRequestModel, Result<IEnumerable<FunFairsDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetFunFairsHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<FunFairsDto>>> Handle(GetFunFairsRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            IEnumerable<FunFairsDto> response = await _context.FunFairs
                .Where(s => !s.IsDeleted &&
                            _context.ServiceCategories.Any(e => e.CityName == request.CityName && e.ServiceId == s.ServiceId && !e.IsDeleted))
                .Select(s => new FunFairsDto
                {
                    FunFairId = s.FunFairId,
                    ResortName = _context.ServiceCategories
                        .Where(e => e.ServiceId == s.ServiceId && e.CityName == request.CityName && !e.IsDeleted)
                        .Select(e => e.ServiceName)
                        .FirstOrDefault()!,
                    CityName = request.CityName
                })
                .OrderBy(s => s.ResortName)
                .ToListAsync(cancellationToken);

            if (!response.Any())
            {
                return Result<IEnumerable<FunFairsDto>>.Failure("Nie znaleziono stoków narciarskich w tym mieście.", ErrorType.NotFound);
            }

            return Result<IEnumerable<FunFairsDto>>.Success(response.AsEnumerable());
        });
    }
}