namespace QrToPay.Api.DTOs
{
    public class SkiSlopeDto
    {
        public int SkiResortId { get; set; }
        public string? ResortName { get; set; }
        public string? CityName { get; set; }
    }

    public class SkiSlopePriceDto
    {
        public int SkiSlopePriceID { get; set; }
        public int Tokens { get; set; }
        public decimal Price { get; set; }
    }
}
