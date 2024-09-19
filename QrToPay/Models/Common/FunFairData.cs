namespace QrToPay.Models.Common;
public sealed class FunFairData
{
    //Lokalny model dla buttonów nie do response z api
    public int FunFairId { get; init; }
    public string? ResortName { get; init; }
    public string? ImageSource { get; init; }
}