using DCS.ApplicationService.Validation;
using DCS.Tests.Bootstrap;
using FluentValidation.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DCS.Tests.Validation
{
    [TestFixture]
    public class IataCodeValidatorTests
    {
        private IataCodeValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = CompositionRoot.ServiceProvider.GetRequiredService<IataCodeValidator>();
        }

        [Test]
        public void ShouldHaveValidationErrorForIncorrectIataCodeLength()
        {
            var iataCode = "XX";
            _validator.TestValidate(iataCode).ShouldHaveValidationErrorFor(x => x).WithErrorMessage("Iata code length should be equal 3");

            iataCode = "XXXX";
            _validator.TestValidate(iataCode).ShouldHaveValidationErrorFor(x => x).WithErrorMessage("Iata code length should be equal 3");
        }

        [Test]
        public void ShouldNotHaveValidationError()
        {
            const string iataCode = "XXX";
            _validator.TestValidate(iataCode).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldNotHaveValidationErrorForIataCodeWithLatinLettersOnly()
        {
            const string iataCode = "XXX";
            _validator.TestValidate(iataCode).ShouldNotHaveAnyValidationErrors();
        }
        
        [Test]
        public void ShouldHaveValidationErrorForIataCodeWithNotOnlyLatinLetters()
        {
            const string iataCode = "АБВ";
            _validator.TestValidate(iataCode).ShouldHaveValidationErrorFor(x => x).WithErrorMessage("Iata code must contains only latin letters");
        }
    }
}