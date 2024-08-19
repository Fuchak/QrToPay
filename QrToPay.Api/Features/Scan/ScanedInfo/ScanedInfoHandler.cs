using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Scan.ScanedInfo;

public class ScanedInfoHandler : IRequestHandler<ScanedInfoRequestModel, Result<ScanedInfoDto>>
{
    private readonly QrToPayDbContext _context;

    public ScanedInfoHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ScanedInfoDto>> Handle(ScanedInfoRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(request.QrCode) || request.QrCode.Length < 8)
            {
                throw new Exception("Nieprawidłowy kod QR. Odczekaj chwilę przed ponownym skanowaniem.");
            }

            var prefix = request.QrCode.Substring(3, 2); // Użycie dwóch liter jako prefixu Chyba to tak powinno chodzić jak trzeba ZAK to 3 i używamy 2 jako te prefixy

            Dictionary<string, Func<Task<ScanedInfoDto>>> handlers = new()
            {
                { "FA", async () => await HandleFunFairAttraction(request.QrCode, cancellationToken) },
                { "SL", async () => await HandleSkiLift(request.QrCode, cancellationToken) },
            };

            return handlers.TryGetValue(prefix, out var handler) 
                ? await handler() 
                : throw new Exception("Nieprawidłowy prefix kodu QR.");
        });
    }

    private async Task<ScanedInfoDto> HandleFunFairAttraction(string qrCode, CancellationToken cancellationToken)
    {
        FunFairAttraction funFairAttractionResponse = await _context.FunFairAttractions
                    .Include(ffa => ffa.FunFair)
                    .FirstOrDefaultAsync(ffa => ffa.Qrcode == qrCode && !ffa.IsDeleted, cancellationToken)
                    ?? throw new Exception("Atrakcja nie została znaleziona.");


        ServiceCategory serviceCategoryResponse = await _context.ServiceCategories
                    .FirstOrDefaultAsync(e => e.ServiceId == funFairAttractionResponse.FunFair.ServiceId && !e.IsDeleted, cancellationToken)
                    ?? throw new Exception("Park nie został znaleziony.");

        return new ScanedInfoDto
        {
            ServiceName = serviceCategoryResponse.ServiceName,
            AttractionName = funFairAttractionResponse.AttractionName,
            ServiceId = serviceCategoryResponse.ServiceId,
            Price = funFairAttractionResponse.TokensPerUse
        };
    }

    private async Task<ScanedInfoDto> HandleSkiLift(string qrCode, CancellationToken cancellationToken)
    {
        SkiLift skiLiftResponse = await _context.SkiLifts
                    .Include(sl => sl.SkiResort)
                    .FirstOrDefaultAsync(sl => sl.Qrcode == qrCode && !sl.IsDeleted, cancellationToken)
                    ?? throw new Exception("Atrakcja nie została znaleziona.");

        ServiceCategory serviceCategoryResponse = await _context.ServiceCategories
                    .FirstOrDefaultAsync(e => e.ServiceId == skiLiftResponse.SkiResort.ServiceId && !e.IsDeleted, cancellationToken)
                    ?? throw new Exception("Ośrodek narciarski nie został znaleziony.");

        return new ScanedInfoDto
        {
            ServiceName = serviceCategoryResponse.ServiceName,
            AttractionName = skiLiftResponse.LiftName,
            ServiceId = serviceCategoryResponse.ServiceId,
            Price = skiLiftResponse.TokensPerUse
        };
    }
}
