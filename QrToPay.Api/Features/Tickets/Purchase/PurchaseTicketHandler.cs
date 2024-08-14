using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Purchase
{
    public class PurchaseTicketHandler : IRequestHandler<PurchaseTicketRequestModel, Result<string>>
    {
        private readonly QrToPayDbContext _context;

        public PurchaseTicketHandler(QrToPayDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(PurchaseTicketRequestModel request, CancellationToken cancellationToken)
        {
            try
            {
                string qrCode;
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync(cancellationToken);
                    using var command = new SqlCommand("dbo.GenerateUniqueUserTicketQRCode", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", request.UserId);

                    var outputParam = new SqlParameter("@QRCode", System.Data.SqlDbType.NVarChar, 255)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    await command.ExecuteNonQueryAsync(cancellationToken);
                    qrCode = outputParam.Value.ToString() ?? string.Empty;
                }

                var parameters = new[]
                {
                    new SqlParameter("@UserID", request.UserId),
                    new SqlParameter("@EntityID", request.EntityId),
                    new SqlParameter("@Quantity", request.Quantity),
                    new SqlParameter("@Tokens", request.Tokens),
                    new SqlParameter("@TotalPrice", request.TotalPrice),
                    new SqlParameter("@QRCode", qrCode)
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateUserTicketsAndAddHistory @UserID, @EntityID, @Quantity, @Tokens, @TotalPrice, @QRCode", parameters);

                return Result<string>.Success(qrCode);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Wewnętrzny błąd serwera: {ex.Message}");
            }
        }
    }
}