using BasicGaming.Api.Models;
using FluentValidation;

namespace BasicGaming.Api.Validators;

public class PaginationInputValidator : AbstractValidator<PaginationInputDTO>
{
    private const int MaxLimit = 999;
    public PaginationInputValidator()
    {
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Offset must be greater than or equal to 0.");

        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Limit must be greater than or equal to 0.")
            .LessThanOrEqualTo(MaxLimit)
            .WithMessage($"Limit must be less than or equal to {MaxLimit}.");
    }
}
