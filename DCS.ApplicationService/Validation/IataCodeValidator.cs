using System.Text.RegularExpressions;
using FluentValidation;

namespace DCS.ApplicationService.Validation
{
    public class IataCodeValidator : AbstractValidator<string>
    {
        public IataCodeValidator()
        {
            RuleFor(x => x)
                .Length(3).WithMessage($"Iata code length should be equal 3");

            RuleFor(x => x)
                .Must(iataCode => Regex.IsMatch(iataCode, "[a-zA-Z]+"))
                .WithMessage("Iata code must contains only latin letters");
        }
    }
}