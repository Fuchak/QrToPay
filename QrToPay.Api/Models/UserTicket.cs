namespace QrToPay.Api.Models
{
    public class UserTicket
    {
        public int UserTicketId { get; set; }
        public int UserID { get; set; }
        public int? TicketId { get; set; }
        public int? AttractionId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; }
        public required string TicketType { get; set; }

        public required User User { get; set; }
    }
}