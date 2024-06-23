using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.DTOs;
using QrToPay.Api.Models;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScanController(QrToPayDbContext context) : ControllerBase
    {
        [HttpGet("{qrCode}")]
        public async Task<IActionResult> GetAttractionByQrCode(string qrCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(qrCode) || qrCode.Length < 5)
                {
                    return BadRequest(new { Message = "Nieprawidłowy kod QR." });
                }

                // Pobierz prefix i resztę kodu
                var prefix = qrCode.Substring(3, 1); // Prefiks jest na czwartym miejscu
                var code = qrCode; // Nie usuwamy prefiksu


                AttractionResponse? response = null;

                // Sprawdź prefix
                if (prefix == "F")
                {
                    // Szukanie atrakcji w FunFairAttractions
                    var funFairAttraction = await context.FunFairAttractions
                        .Include(ffa => ffa.FunFair) // Dodajemy Include, aby pobrać powiązaną nazwę wesołego miasteczka
                        .FirstOrDefaultAsync(ffa => ffa.Qrcode == qrCode && !ffa.IsDeleted);

                    if (funFairAttraction != null)
                    {
                        response = new AttractionResponse
                        {
                            Type = funFairAttraction.FunFair.ParkName, // Używamy nazwy wesołego miasteczka jako typu
                            AttractionName = funFairAttraction.AttractionName,
                            Price = funFairAttraction.TokensPerUse // Cena w tokenach, dostosuj według potrzeb
                        };
                    }
                }
                else if (prefix == "S")
                {
                    // Szukanie atrakcji w SkiLifts
                    var skiLift = await context.SkiLifts
                        .Include(sl => sl.SkiResort) // Dodajemy Include, aby pobrać powiązaną nazwę ośrodka narciarskiego
                        .FirstOrDefaultAsync(sl => sl.Qrcode == qrCode && !sl.IsDeleted);

                    if (skiLift != null)
                    {
                        response = new AttractionResponse
                        {
                            Type = skiLift.SkiResort.ResortName, // Używamy nazwy ośrodka narciarskiego jako typu
                            AttractionName = skiLift.LiftName,
                            Price = skiLift.TokensPerUse // Cena w tokenach, dostosuj według potrzeb
                        };
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Nieprawidłowy prefix kodu QR." });
                }

                if (response != null)
                {
                    return Ok(response);
                }

                return NotFound(new { Message = "Atrakcja nie została znaleziona." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Wystąpił błąd serwera: {ex.Message}" });
            }
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseTicket(PurchaseRequest request)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
                if (user == null)
                {
                    return NotFound(new { Message = "Użytkownik nie został znaleziony." });
                }

                if (user.AccountBalance < request.Price)
                {
                    return BadRequest(new { Message = "Niewystarczające środki na koncie." });
                }

                var funFair = await context.FunFairs.FirstOrDefaultAsync(ff => ff.ParkName == request.Type);
                var skiResort = await context.SkiSlopes.FirstOrDefaultAsync(sr => sr.ResortName == request.Type);

                user.AccountBalance -= request.Price;

                var purchase = new Models.TicketHistory
                {
                    UserId = request.UserId,
                    FunFairId = funFair?.FunFairId,
                    SkiResortId = skiResort?.SkiResortId,
                    PurchaseDate = DateTime.Now,
                    Quantity = 1,
                    TotalPrice = request.Price
                };

                context.TicketHistories.Add(purchase);
                await context.SaveChangesAsync();
                
                return Ok(new { Message = "Zakup został dodany do historii." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Wystąpił błąd serwera: {ex.Message}" });
            }
        }
    }
}
