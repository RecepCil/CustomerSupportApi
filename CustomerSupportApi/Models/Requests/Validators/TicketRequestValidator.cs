using FluentValidation;
using FluentValidation.Results;

namespace CustomerSupportApi.Models.Requests.Validators;

public class TicketRequestValidator : AbstractValidator<TicketRequest>
{
    protected override bool PreValidate(ValidationContext<TicketRequest> context, ValidationResult result)
    {
        if (context.InstanceToValidate == null)
        {
            result.Errors.Add(new ValidationFailure("Model", "Supplied model is not valid!"));
            return false;
        }
            
        return true;
    }

    public TicketRequestValidator()
    {
        CascadeMode = FluentValidation.CascadeMode.Stop;
            
        RuleFor(r => r.Type)
            .IsInEnum();

        RuleFor(r => r.CustomerEmail)
            .NotEmpty()
            .WithMessage("Email address is required!")
            .EmailAddress()
            .WithMessage("A valid email address is required!");

        RuleFor(r => r.CustomerPhone)
            .NotEmpty()
            .WithMessage("Phone number is required!");

        When(r => !string.IsNullOrWhiteSpace(r.CustomerNumber), () =>
        {
            RuleFor(r => r.CustomerNumber)
                .Custom((x, context) =>
                {
                    if ((!(int.TryParse(x, out int value)) || value < 0))
                    {
                        context.AddFailure($"{x} is not a valid number or less than 0!");
                    }
                });
        });

        RuleFor(r => r.Description)
            .NotEmpty()
            .MaximumLength(500);
    }
}
