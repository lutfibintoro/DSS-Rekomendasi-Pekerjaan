using BackEnd.Common.Errors;
using FluentValidation;

namespace BackEnd.Features.Auth.Register;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username wajib diisi")
            .MinimumLength(3).MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email wajib diisi")
            .EmailAddress().WithMessage("Format email tidak valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password wajib diisi")
            .MinimumLength(6).WithMessage("Password minimal 6 karakter");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role wajib diisi")
            .Must(r => r == "admin" || r == "pelamar")
            .WithMessage("Role harus 'admin' atau 'pelamar'");
    }
}