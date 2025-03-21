using FluentValidation;
using Nexus.Application.Auth.Dtos;

namespace Nexus.Application.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest> 
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required!")
            .EmailAddress().WithMessage("Not a valid email!");
        
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage("Username is required!")
            .Length(4,16).WithMessage("Username length must be within 4 and 16 characters long");
    }
}