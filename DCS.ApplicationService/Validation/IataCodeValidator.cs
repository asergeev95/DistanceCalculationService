using FluentValidation;

namespace DCS.ApplicationService.Validation
{
    public class IataCodeValidator : AbstractValidator<string>
    {
        public IataCodeValidator()
        {
            RuleFor(x => x)
                .Length(3);
        }
    }
}