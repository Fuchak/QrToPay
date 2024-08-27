using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Purchase;

public class PurchaseTicketHandler : IRequestHandler<PurchaseTicketRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;

    public PurchaseTicketHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(PurchaseTicketRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            Guid token;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);

                using var command = new SqlCommand("dbo.UpdateUserTicketsAndGenerateToken", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserID", request.UserId);
                command.Parameters.AddWithValue("@ServiceID", request.ServiceId);
                command.Parameters.AddWithValue("@Quantity", request.Quantity);
                command.Parameters.AddWithValue("@Tokens", request.Tokens);
                command.Parameters.AddWithValue("@TotalPrice", request.TotalPrice);

                SqlParameter response = new("@Token", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                command.Parameters.Add(response);

                await command.ExecuteNonQueryAsync(cancellationToken);
                token = (Guid)response.Value;
            }

            return Result<string>.Success(token.ToString());
        });
    }
}