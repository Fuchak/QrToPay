﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Settings.Password;
using QrToPay.Api.Features.Settings.Verify;
using QrToPay.Api.Features.Settings.EmailPhone;

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

        [HttpPost("requestChange")]
        public async Task<IActionResult> RequestChange([FromBody] ChangeRequestModel request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(new { VerificationCode = result.Value });
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(new { Message = result.Value });
        }


        [HttpPost("verifyChange")]
        public async Task<IActionResult> VerifyChange([FromBody] VerifyRequestModel request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(new { Message = result.Value });
        }
    }
}