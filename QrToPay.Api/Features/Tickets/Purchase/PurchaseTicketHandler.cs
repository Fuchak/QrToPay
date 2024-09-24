using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.Tickets.Purchase;

public class PurchaseTicketHandler : IRequestHandler<PurchaseTicketRequestModel, Result<PurchaseTicketDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public PurchaseTicketHandler(QrToPayDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PurchaseTicketDto>> Handle(PurchaseTicketRequestModel request, CancellationToken cancellationToken)
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

                command.Parameters.AddWithValue("@UserID", _currentUserService.UserId);
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

            return Result<PurchaseTicketDto>.Success(new() { QrCode = token.ToString() });
        });
    }
}