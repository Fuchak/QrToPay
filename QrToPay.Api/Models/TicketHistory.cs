namespace QrToPay.Api.Models;

public partial class TicketHistory
{
    public int HistoryId { get; set; }

    public int UserId { get; set; }

    public Guid EntityId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
