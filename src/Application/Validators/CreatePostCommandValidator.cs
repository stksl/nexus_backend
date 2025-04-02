using FluentValidation;

namespace Nexus.Application;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        const int contentSize = 2 << 14;
        RuleFor(cp => cp.Content)
            .NotEmpty().WithMessage("Content can't be empty!")
            .MaximumLength(contentSize).WithMessage("Maximum content length is 16384 characters!");
        
        RuleFor(cp => cp.Headline)
            .NotEmpty().WithMessage("Headline can't be empty!")
            .MaximumLength(255).WithMessage("Maximum headline length is 255 characters");

        RuleFor(cp => cp.Tags.Count())
            .LessThanOrEqualTo(5).WithMessage("A post can only have 5 tags!");
        
        RuleFor(cp => cp.Tags).Must(BeLower).WithMessage("All tags must be lowered!");
    }

    private bool BeLower(IEnumerable<string> tags) 
    {
        return tags.FirstOrDefault(t => t.ToLower() != t) == null;
    }
}