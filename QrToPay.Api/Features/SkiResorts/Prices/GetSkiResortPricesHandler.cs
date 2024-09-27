using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.SkiResorts.Prices;

public class GetSkiResortPricesHandler : IRequestHandler<GetSkiResortPricesRequestModel, Result<IEnumerable<SkiResortPriceDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetSkiResortPricesHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<SkiResortPriceDto>>> Handle(GetSkiResortPricesRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            IEnumerable<SkiResortPriceDto> response = await _context.SkiResortPrices
                            .Where(p => p.SkiResortId == request.SkiResortId && !p.IsDeleted)
                            .Select(p => new { p.Tokens, p.Price, p.SkiResortPriceId })
                            .Distinct()
                            .Select(p => new SkiResortPriceDto
                            {
                                SkiResortPriceId = p.SkiResortPriceId,
                                Tokens = p.Tokens,
                                Price = p.Price
                            })
                            .OrderBy(p => p.Price)
                            .ToListAsync(cancellationToken);

            if (!response.Any())
            {
                return Result<IEnumerable<SkiResortPriceDto>>.Failure("Nie znaleziono ofert zakupu biletów dla tego resortu.", ErrorType.NotFound);
            }

            return Result<IEnumerable<SkiResortPriceDto>>.Success(response.AsEnumerable());
        });
    }
}