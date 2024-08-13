﻿using MediatR;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Auth.ResetPassword;

public sealed class UserExistRequestModel : IRequest<Result<string>>
{
    public string? Contact { get; init; }
    public required ChangeType ChangeType { get; init; }
}