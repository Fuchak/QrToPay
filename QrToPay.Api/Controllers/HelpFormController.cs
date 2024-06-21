using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Models;
using QrToPay.Api.DTOs;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelpFormController(QrToPayDbContext context) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateHelpForm(HelpFormRequest request)
        {
            var helpForm = new HelpForm
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

            context.HelpForms.Add(helpForm);
            await context.SaveChangesAsync();

            return Ok(new { Message = "Zgłoszenie zostało utworzone." });
        }
    }
}
