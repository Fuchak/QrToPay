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
        try
        {
            if (string.IsNullOrWhiteSpace(request.QrCode) || request.QrCode.Length < 5)
            {
                return Result<ScanedInfoDto>.Failure("Nieprawidłowy kod QR. Odczekaj chwilę przed ponownym skanowaniem.");
            }

            var prefix = request.QrCode.Substring(3, 1);

            if (prefix == "F")
            {
                var funFairAttraction = await _context.FunFairAttractions
                    .Include(ffa => ffa.FunFair)
                    .FirstOrDefaultAsync(ffa => ffa.Qrcode == request.QrCode && !ffa.IsDeleted, cancellationToken);

                if (funFairAttraction != null)
                {
                    var entity = await _context.ServiceCategories
                        .FirstOrDefaultAsync(e => e.ServiceId == funFairAttraction.FunFair.ServiceId && !e.IsDeleted, cancellationToken);

                    if (entity == null)
                    {
                        return Result<ScanedInfoDto>.Failure("Park nie został znaleziony.");
                    }

                    ScanedInfoDto response = new()
                    {
                        ServiceName = entity.ServiceName,
                        AttractionName = funFairAttraction.AttractionName,
                        ServiceId = entity.ServiceId,
                        Price = funFairAttraction.TokensPerUse
                    };
                    return Result<ScanedInfoDto>.Success(response);
                }
            }
            else if (prefix == "S")
            {
                var skiLift = await _context.SkiLifts
                    .Include(sl => sl.SkiResort)
                    .FirstOrDefaultAsync(sl => sl.Qrcode == request.QrCode && !sl.IsDeleted, cancellationToken);

                if (skiLift != null)
                {
                    var entity = await _context.ServiceCategories
                        .FirstOrDefaultAsync(e => e.ServiceId == skiLift.SkiResort.ServiceId && !e.IsDeleted, cancellationToken);

                    if (entity == null)
                    {
                        return Result<ScanedInfoDto>.Failure("Ośrodek narciarski nie został znaleziony.");
                    }

                    ScanedInfoDto response = new()
                    {
                        ServiceName = entity.ServiceName,
                        AttractionName = skiLift.LiftName,
                        ServiceId = entity.ServiceId,
                        Price = skiLift.TokensPerUse
                    };
                    return Result<ScanedInfoDto>.Success(response);
                }
            }
            else
            {
                return Result<ScanedInfoDto>.Failure("Nieprawidłowy prefix kodu QR.");
            }

            return Result<ScanedInfoDto>.Failure("Atrakcja nie została znaleziona. Odczekaj chwilę przed ponownym skanowaniem.");
        }
        catch (Exception ex)
        {
            return Result<ScanedInfoDto>.Failure($"Wystąpił błąd serwera: {ex.Message}");
        }
    }
}
