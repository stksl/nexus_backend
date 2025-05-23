using FluentValidation;

namespace Nexus.Application;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        const int contentSize = 2 << 12;
        RuleFor(cp => cp.Content)
            .NotEmpty().WithMessage("Content can't be empty!")
            .MaximumLength(contentSize).WithMessage("Maximum content length is 4096 characters!");
    }
}