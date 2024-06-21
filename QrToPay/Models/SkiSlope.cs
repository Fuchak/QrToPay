namespace QrToPay.Models;
public class SkiSlope
{
    public int SkiResortId { get; set; }
    public string? ResortName { get; set; }
    public string? ImageSource { get; set; }
}

public class SkiSlopePrice
{
    public int SkiSlopePriceID { get; set; }
    public int Tokens { get; set; }
    public decimal Price { get; set; }
    public string? LiftName { get; set; }
}