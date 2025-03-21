using FluentValidation;
using Nexus.Application.Auth.Dtos;

namespace Nexus.Application.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest> 
{
    public LoginRequestValidator()
    {
        RuleFor(l => l.Password)
            .NotEmpty().WithMessage("Password is required!");

        When(l => l.IsEmailIdentifier, () => 
        {
            RuleFor(l => l.Identifier)
                .NotEmpty().WithMessage("Email is required!")
                .EmailAddress().WithMessage("Not a valid email!");
        }).Otherwise(() => 
        {
            RuleFor(l => l.Identifier)
                .NotEmpty().WithMessage("Username is required!")
                .Length(4, 16).WithMessage("Username length must be within 4 and 16 characters long");
        });
    }
}