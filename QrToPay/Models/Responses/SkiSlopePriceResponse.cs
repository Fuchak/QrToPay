﻿namespace QrToPay.Models.Responses;

public sealed class SkiSlopePriceResponse
{
    //public int SkiSlopePriceId { get; init; }
    public required int Tokens { get; init; }
    public required decimal Price { get; init; }
    //public string? LiftName { get; init; }
}