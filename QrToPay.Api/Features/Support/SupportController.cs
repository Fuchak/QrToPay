using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Features.Support.HelpFormFeature;
using QrToPay.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace QrToPay.Api.Features.Support
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public SupportController(QrToPayDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateHelpForm([Required] HelpFormRequestModel request) //Tu można dawać required tu albo w modelu
        {
            HelpForm helpForm = new()
            {
                UserName = request.UserName,
                UserEmail = request.UserEmail,
                Subject = request.Subject,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };

            _context.HelpForms.Add(helpForm);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Zgłoszenie zostało utworzone." });
        }
    }
}