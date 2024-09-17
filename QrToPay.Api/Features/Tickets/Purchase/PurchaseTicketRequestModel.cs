﻿using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Purchase;

public class PurchaseTicketRequestModel : IRequest<Result<SuccesMessageDto>>
{
    public required int UserId { get; init; }
    public required Guid ServiceId { get; init; }
    public required int Quantity { get; init; }
    public required int Tokens { get; init; }
    public required decimal TotalPrice { get; init; }
}