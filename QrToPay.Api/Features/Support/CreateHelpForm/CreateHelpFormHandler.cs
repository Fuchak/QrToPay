﻿using MediatR;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Support.CreateHelpForm;

public class CreateHelpFormHandler : IRequestHandler<CreateHelpFormRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;

    public CreateHelpFormHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(CreateHelpFormRequestModel request, CancellationToken cancellationToken)
    {
        var helpForm = new HelpForm
        {
            UserName = request.UserName,
            UserEmail = request.UserEmail,
            Subject = request.Subject,
            Description = request.Description,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.HelpForms.Add(helpForm);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("Zgłoszenie zostało utworzone.");
    }
}