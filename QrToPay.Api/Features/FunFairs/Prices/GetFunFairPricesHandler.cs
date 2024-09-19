using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.FunFairs.Prices;

public class GetFunFairPricesHandler : IRequestHandler<GetFunFairPricesRequestModel, Result<IEnumerable<FunFairPriceDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetFunFairPricesHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<FunFairPriceDto>>> Handle(GetFunFairPricesRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            IEnumerable<FunFairPriceDto> response = await _context.FunFairPrices
                            .Where(p => p.FunFairId == request.FunFairId && !p.IsDeleted)
                            .Select(p => new { p.Tokens, p.Price, p.FunFairPriceId })
                            .Distinct()
                            .Select(p => new FunFairPriceDto
                            {
                                FunFairPriceId = p.FunFairPriceId,
                                Tokens = p.Tokens,
                                Price = p.Price
                            })
                            .OrderBy(p => p.Price)
                            .ToListAsync(cancellationToken);

            if (!response.Any())
            {
                return Result<IEnumerable<FunFairPriceDto>>.Failure("Nie znaleziono ofert zakupu biletów dla tego resortu.", ErrorType.NotFound);
            }

            return Result<IEnumerable<FunFairPriceDto>>.Success(response.AsEnumerable());
        });
    }
}