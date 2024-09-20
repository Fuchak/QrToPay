﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QrToPay.Api.Common.Settings;

namespace QrToPay.Api.Middleware;
public class UserContextMiddleware : IMiddleware
{
    private readonly IOptions<AppAuthSettings> _appSettings;

    public UserContextMiddleware(IOptions<AppAuthSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //string? token = context.Request.Headers.Authorization;
        if (token is not null)
        {
            try
            {
                AttachUserToContext(context, token);
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd weryfikacji tokena JWT.", ex);
            }
        }
        await next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetTokenValidationParameters();

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
        var phoneNumberClaim = principal.FindFirst("phone_number")?.Value;

        if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
        {
            context.Items["UserId"] = userId;
            context.Items["UserEmail"] = emailClaim;
            context.Items["UserPhoneNumber"] = phoneNumberClaim;
        }
}

    private TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Secret)),
            ValidateIssuer = !string.IsNullOrEmpty(_appSettings.Value.Issuer),
            ValidateAudience = !string.IsNullOrEmpty(_appSettings.Value.Audience),
            ValidIssuer = _appSettings.Value.Issuer,
            ValidAudience = _appSettings.Value.Audience,
            ValidateLifetime = true
        };
    }
}