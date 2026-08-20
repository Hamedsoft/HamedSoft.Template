using FluentValidation;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Register;

public sealed class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z0-9\-\._@+]+$")
            .WithMessage("نام کاربری فقط می‌تواند شامل حروف انگلیسی، اعداد و کاراکترهای - . _ @ + باشد");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);
    }
}