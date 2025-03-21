using FluentValidation;
using Nexus.Application.Auth.Dtos;

namespace Nexus.Application.Auth;

public class EmailConfirmRequestValidator : AbstractValidator<EmailConfirmRequest> 
{
    public EmailConfirmRequestValidator()
    {
        RuleFor(r => r.EmailToken)
            .NotEmpty().WithMessage("Email token is required!");
        
        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("User id is required!");
    }
}