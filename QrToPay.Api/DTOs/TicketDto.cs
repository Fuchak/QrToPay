namespace QrToPay.Api.DTOs
{
    public class TicketResponse
    {
        public int UserTicketId { get; set; }
        public int UserId { get; set; }
        public int? SkiResortId { get; set; }
        public int? FunFairId { get; set; }
        public string? ResortName { get; set; }
        public string? CityName { get; set; }
        public string? QrCode { get; set; }
        public decimal Price { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateTicketRequest
    {
        public int UserID { get; set; }
        public int? SkiResortID { get; set; }
        public int? FunFairID { get; set; }
        public int Quantity { get; set; }
        public int Tokens { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class TicketHistory
    {
        public string? Date { get; set; }
        public string? Type { get; set; }
        public decimal TotalPrice { get; set; }
    }

    //To do skanowania dodane tutaj tak na szybko
    public class PurchaseRequest
    {
        public int UserId { get; set; }
        public string? Type { get; set; }
        public string? AttractionName { get; set; }
        public decimal Price { get; set; }
    }
}
