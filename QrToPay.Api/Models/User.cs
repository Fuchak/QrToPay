using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? VerificationCode { get; set; }

    public decimal? AccountBalance { get; set; }

    public bool IsVerified { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
