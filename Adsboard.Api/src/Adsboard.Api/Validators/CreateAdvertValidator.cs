using Adsboard.Api.Messages.Adverts;
using FluentValidation;

namespace Adsboard.Api.Validators
{
    public class CreateAdvertValidator : AbstractValidator<CreateAdvert>
    {
        public CreateAdvertValidator()
        {
            RuleFor(x => x.Title).MinimumLength(10);
            RuleFor(x => x.Description).MinimumLength(30);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}