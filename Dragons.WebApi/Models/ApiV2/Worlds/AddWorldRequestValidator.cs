using FluentValidation;

namespace Dragons.WebApi.Models.ApiV2.Worlds
{
    //validator class for AddWorldRequest
    public class AddWorldRequestValidator:AbstractValidator<AddWorldRequest>
    {
        public AddWorldRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("'Name' is required");
            When(o => !string.IsNullOrWhiteSpace(o.Name), () => 
            {
                RuleFor(o => o.Name).MaximumLength(100).WithMessage("Max length of 'Name' is 100");
            });
        }
    }
}
