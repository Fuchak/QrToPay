﻿namespace QrToPay.Models.Common;
public sealed class SkiResortData
{
    //Lokalny model dla buttonów nie do response z api
    public int SkiResortId { get; init; }
    public string? ResortName { get; init; }
    public string? ImageSource { get; init; }
}