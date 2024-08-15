using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.SkiSlopes.Slopes;

public class GetSkiSlopesHandler : IRequestHandler<GetSkiSlopesRequestModel, Result<IEnumerable<SkiSlopesDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetSkiSlopesHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<SkiSlopesDto>>> Handle(GetSkiSlopesRequestModel request, CancellationToken cancellationToken)
    {
        var skiSlopes = await _context.SkiSlopes
            .Where(s => !s.IsDeleted && s.EntityId == request.EntityId)
            .Select(s => new SkiSlopesDto
            {
                SkiResortId = s.SkiResortId,
                ResortName = _context.Entities
                    .Where(e => e.EntityId == s.EntityId && !e.IsDeleted)
                    .Select(e => e.EntityName)
                    .FirstOrDefault()!,
                CityName = _context.Entities
                    .Where(e => e.EntityId == s.EntityId && !e.IsDeleted)
                    .Select(e => e.CityName)
                    .FirstOrDefault()!,
            })
            .ToListAsync(cancellationToken);

        if (skiSlopes.Count == 0)
        {
            return Result<IEnumerable<SkiSlopesDto>>.Failure($"Nie znalezionio stoków narciarskich w tym mieście");
        }

        return Result<IEnumerable<SkiSlopesDto>>.Success(skiSlopes);
    }
}