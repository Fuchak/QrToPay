﻿using QrToPay.Models.Common;

namespace QrToPay.Models.Requests;

public sealed class PurchaseRequest
{
    public required string ServiceName { get; init; }
    public required int ServiceId { get; init; }
    public required string AttractionName { get; init; }
    public required decimal Price { get; init; }
}