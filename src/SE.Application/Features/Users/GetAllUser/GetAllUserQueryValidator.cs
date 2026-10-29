using FluentValidation;

namespace SE.Application.Features.Users.GetAllUser;

public class GetAllUserQueryValidator : AbstractValidator<GetAllUserQuery>
{
    public GetAllUserQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0.");
        RuleFor(x => x.PageSize)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must not exceed 100.");
        RuleFor(x => x.Search).MaximumLength(256);
        RuleFor(x => x.Role).MaximumLength(256);
    }
}
