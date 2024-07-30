namespace QrToPay.Api.Models;

public partial class UserTicket
{
    public int UserTicketId { get; set; }

    public int UserId { get; set; }

    public Guid EntityId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public int RemainingTokens { get; set; }

    public bool IsActive { get; set; }

    public string Qrcode { get; set; } = null!;

    public virtual Entity Entity { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
