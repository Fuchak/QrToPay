﻿using QrToPay.Api.Common.Enums;
using System.Collections.Generic;

namespace QrToPay.Api.Features.Tickets.Active;

public sealed class ActiveTicketDto
{
    public required int UserTicketId { get; init; }
    public required int GroupId { get; init; }
    //public required ServiceType EntityType { get; init; }
    public required IEnumerable<string> EntityNames { get; init; }
    //public required string CityName { get; init; }
    public required string QrCode { get; init; }
    public required decimal Price { get; init; }
    public required int Points { get; init; }
    public required bool IsActive { get; init; }
}