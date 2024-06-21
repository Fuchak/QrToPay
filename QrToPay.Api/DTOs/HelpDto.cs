namespace QrToPay.Api.DTOs
{
    public class HelpFormRequest
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } // Domyślnie "Nowe"
    }
}