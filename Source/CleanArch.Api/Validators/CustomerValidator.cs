using FluentValidation;
using FluentValidation.Results;
using CleanArch.Domain.Dtos;

namespace CleanArch.Api.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Name).NotEmpty();
        }

        protected override void RaiseValidationException(ValidationContext<CustomerDto> context, ValidationResult result)
        {
            base.RaiseValidationException(context, result);
        }
    }
}