namespace QrToPay.Models
{
    public class Ticket
    {
        public string? CityName { get; set; }
        public int UserId { get; set; }
        public string? ResortName { get; set; }
        public string? QrCode { get; set; }
        public decimal Price { get; set; }
        public int Points { get; set; }
    }


    public class UpdateTicketRequest
    {
        public int UserID { get; set; }
        public int? SkiResortID { get; set; }
        public int? FunFairID { get; set; }
        public int Quantity { get; set; }
        public int Tokens { get; set; }
        public string? TotalPrice { get; set; }
    }

    public class UpdateTicketResponse
    {
        public string? QrCode { get; set; }
    }

    public class HistoryItem
    {
        public string? Date { get; set; }
        public string? Type { get; set; }
        public decimal TotalPrice { get; set; }
    }

    //Tu też dodane na szybko dla biletów w sumie można by to poprostu w shared dać
    public class PurchaseRequest
    {
        public int UserId { get; set; }
        public string? Type { get; set; }
        public string? AttractionName { get; set; }
        public decimal Price { get; set; }
    }
}
