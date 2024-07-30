using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Text.Json;
using QrToPay.Api.Responses;
using QrToPay.Api.Requests;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public TicketsController(QrToPayDbContext context)
        {
            _context = context;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveTickets([FromQuery] int userId)
        {
            var activeTickets = await _context.UserTickets
                .Where(t => t.UserId == userId && t.IsActive)
                .Include(t => t.Entity)
                .ToListAsync();

            var ticketResponses = activeTickets.Select(t => new TicketDto
            {
                UserTicketId = t.UserTicketId,
                UserId = t.UserId,
                EntityId = t.EntityId,
                EntityType = t.Entity.EntityType,
                EntityName = t.Entity.EntityName,
                CityName = t.Entity.CityName,
                QrCode = t.Qrcode,
                Price = t.TotalPrice,
                Points = t.RemainingTokens,
                IsActive = t.IsActive
            }).ToList();

            return Ok(ticketResponses);
        }

        [HttpPost("generateAndUpdate")]
        public async Task<IActionResult> GenerateAndUpdateTicket([FromBody] UpdateTicketRequestModel request)
        {
            try
            {
                string qrCode;
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    using var command = new SqlCommand("dbo.GenerateUniqueUserTicketQRCode", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", request.UserId);

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
                    new SqlParameter("@UserID", request.UserId),
                    new SqlParameter("@EntityID", request.EntityId),
                    new SqlParameter("@Quantity", request.Quantity),
                    new SqlParameter("@Tokens", request.Tokens),
                    new SqlParameter("@TotalPrice", request.TotalPrice),
                    new SqlParameter("@QRCode", qrCode)
                };

                Debug.WriteLine($"Executing Stored Procedure: UserID={request.UserId}, EntityID={request.EntityId}, Quantity={request.Quantity}, Tokens={request.Tokens}, TotalPrice={request.TotalPrice}, QRCode={qrCode}");

                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateUserTicketsAndAddHistory @UserID, @EntityID, @Quantity, @Tokens, @TotalPrice, @QRCode", parameters);

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
                var ticketHistory = await _context.TicketHistories
                    .Where(th => th.UserId == userId)
                    .OrderByDescending(th => th.PurchaseDate)
                    .Select(th => new TicketHistoryDto
                    {
                        Date = th.PurchaseDate.ToString("yyyy-MM-dd"),
                        Type = th.Entity.EntityType,
                        Name = th.Entity.EntityName,
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