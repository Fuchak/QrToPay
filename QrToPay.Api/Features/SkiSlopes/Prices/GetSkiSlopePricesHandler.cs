using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.SkiSlopes.Prices;

public class GetSkiSlopePricesHandler : IRequestHandler<GetSkiSlopePricesRequestModel, Result<IEnumerable<SkiSlopePriceDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetSkiSlopePricesHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<SkiSlopePriceDto>>> Handle(GetSkiSlopePricesRequestModel request, CancellationToken cancellationToken)
    {
        var prices = await _context.SkiSlopePrices
            .Where(p => p.SkiResortId == request.SkiResortId && !p.IsDeleted)
            .Select(p => new { p.Tokens, p.Price, p.SkiSlopePriceId })
            .Distinct()
            .Select(p => new SkiSlopePriceDto
            {
                SkiSlopePriceId = p.SkiSlopePriceId,
                Tokens = p.Tokens,
                Price = p.Price
            })
            .ToListAsync(cancellationToken);

        if (prices.Count == 0)
        {
            return Result<IEnumerable<SkiSlopePriceDto>>.Failure($"Nie znaleziono ofer zakupu biletów dla tego resortu");
        }

        return Result<IEnumerable<SkiSlopePriceDto>>.Success(prices);
    }
}