using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Models;
using QrToPay.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController(QrToPayDbContext context) : ControllerBase
    {

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveTickets([FromQuery] int userId)
        {
            var activeTickets = await context.UserTickets
                .Where(t => t.UserId == userId && t.IsActive)
                .Include(t => t.SkiResort)
                    .ThenInclude(sr => sr.City)
                .Include(t => t.FunFair)
                    .ThenInclude(ff => ff.City)
                .ToListAsync();

            var ticketResponses = activeTickets.Select(t => new TicketResponse
            {
                UserTicketId = t.UserTicketId,
                UserId = t.UserId,
                SkiResortId = t.SkiResortId,
                FunFairId = t.FunFairId,
                ResortName = t.SkiResort?.ResortName ?? t.FunFair?.ParkName ?? string.Empty,
                CityName = t.SkiResort != null ? t.SkiResort.City?.CityName : t.FunFair?.City?.CityName ?? string.Empty,
                QrCode = t.Qrcode ?? string.Empty,
                Price = t.TotalPrice,
                Points = t.RemainingTokens,
                IsActive = t.IsActive
            }).ToList();

            return Ok(ticketResponses);
        }

        [HttpPost("generateAndUpdate")]
        public async Task<IActionResult> GenerateAndUpdateTicket([FromBody] UpdateTicketRequest request)
        {
            try
            {
                string qrCode;
                using (var connection = new SqlConnection(context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    using var command = new SqlCommand("dbo.GenerateUniqueUserTicketQRCode", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", request.UserID);

                    var outputParam = new SqlParameter("@QRCode", System.Data.SqlDbType.NVarChar, 255)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    await command.ExecuteNonQueryAsync();
                    qrCode = outputParam.Value.ToString() ?? string.Empty;
                }

                Debug.WriteLine($"Generated QR Code: {qrCode}");

                // Aktualizacja lub dodanie biletu
                var parameters = new[]
                {
                    new SqlParameter("@UserID", request.UserID),
                    new SqlParameter("@SkiResortID", request.SkiResortID ?? (object)DBNull.Value),
                    new SqlParameter("@FunFairID", request.FunFairID ?? (object)DBNull.Value),
                    new SqlParameter("@Quantity", request.Quantity),
                    new SqlParameter("@Tokens", request.Tokens),
                    new SqlParameter("@TotalPrice", request.TotalPrice),
                    new SqlParameter("@QRCode", qrCode)
                };

                Debug.WriteLine($"Executing Stored Procedure: UserID={request.UserID}, SkiResortID={request.SkiResortID}, FunFairID={request.FunFairID}, Quantity={request.Quantity}, Tokens={request.Tokens}, TotalPrice={request.TotalPrice}, QRCode={qrCode}");

                await context.Database.ExecuteSqlRawAsync("EXEC UpdateUserTicketsAndAddHistory @UserID, @SkiResortID, @FunFairID, @Quantity, @Tokens, @TotalPrice, @QRCode", parameters);

                return Ok(new { qrCode });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Wewnętrzny błąd serwera: {ex.Message}");
                return StatusCode(500, $"Wewnętrzny błąd serwera: {ex.Message}");
            }
        }

        [HttpGet("getHistory/{userId}")]
        public async Task<IActionResult> GetTicketHistory(int userId)
        {
            try
            {
                var ticketHistory = await context.TicketHistories
                    .Where(th => th.UserId == userId)
                    .OrderByDescending(th => th.PurchaseDate)
                    .Select(th => new DTOs.TicketHistory
                    {
                        Date = th.PurchaseDate.ToString("yyyy-MM-dd"),
                        Type = th.SkiResortId.HasValue
                            ? context.SkiSlopes.Where(sr => sr.SkiResortId == th.SkiResortId).Select(sr => sr.ResortName).FirstOrDefault()
                            : th.FunFairId.HasValue
                                ? context.FunFairs.Where(ff => ff.FunFairId == th.FunFairId).Select(ff => ff.ParkName).FirstOrDefault()
                                : "UNKNOWN",
                        TotalPrice = th.TotalPrice
                    })
                    .ToListAsync();

                Debug.WriteLine($"Response data: {JsonSerializer.Serialize(ticketHistory)}");

                return Ok(ticketHistory);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
