using FluentValidation;
using Nexus.Application.Dtos;

namespace Nexus.Application;

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