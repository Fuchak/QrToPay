using FluentValidation;

namespace QrToPay.Api.Features.Scan.ScanedInfo;

public class ScanedInfoRequestModelValidator : AbstractValidator<ScanedInfoRequestModel>
{
    public ScanedInfoRequestModelValidator()
    {
        RuleFor(x => x.QrCode)
            .NotEmpty()
            .WithMessage("Kod QR jest wymagany.")

            .MinimumLength(11)
            .WithMessage("Kod QR musi mieć co najmniej 11 znaków.");
    }
}