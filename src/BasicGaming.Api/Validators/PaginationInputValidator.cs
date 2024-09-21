using BasicGaming.Api.Models;
using FluentValidation;

namespace BasicGaming.Api.Validators;

public class PaginationAInputValidator : AbstractValidator<PaginationAInputDTO>
{
    private const int MaxLimit = 999;
    public PaginationAInputValidator()
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

public class PaginationBInputValidator : AbstractValidator<PaginationBInputDTO>
{
    private const int MaxPageSize = 50;
    public PaginationBInputValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage($"PageSize must be less than or equal to {MaxPageSize}.");

        RuleFor(x => x.OrderBy)
            .Must(x => new[] { "asc","desc" }.Contains(x))
            .WithMessage("OrderBy should be either 'asc' or 'desc'");
    }
}
