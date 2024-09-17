using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Settings.Password;
using QrToPay.Api.Features.Settings.Verify;
using QrToPay.Api.Features.Settings.EmailPhone;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Settings
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary> Change phone or email </summary>
        /// <response code="404">Not Found </response>
        /// <response code="400">Validation error </response>
        /// <response code="200">Success </response>
        [HttpPost("requestChange")]
        public async Task<IActionResult> RequestChange([FromBody] ChangeEmailPhoneRequestModel request)
        {
            Result<SuccesMessageDto> result = await _mediator.Send(request);

            return result.ToActionResult();
        }

        /// <summary> Change Password </summary>
        /// <response code="404">Not Found </response>
        /// <response code="400">Validation error </response>
        /// <response code="200">Success </response>
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel request)
        {
            Result<SuccesMessageDto> result = await _mediator.Send(request);

            return result.ToActionResult();
        }

        /// <summary> Verify change of password, phone or email</summary>
        /// <response code="404">Not Found </response>
        /// <response code="400">Validation error </response>
        /// <response code="200">Success </response>
        [HttpPost("verifyChange")]
        public async Task<IActionResult> VerifyChange([FromBody] VerifyRequestModel request)
        {
            Result<SuccesMessageDto> result = await _mediator.Send(request);

            return result.ToActionResult();
        }
    }
}