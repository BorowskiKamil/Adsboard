using Adsboard.Api.Messages.Adverts;
using FluentValidation;

namespace Adsboard.Api.Validators
{
    public class UpdateAdvertValidator : AbstractValidator<UpdateAdvert>
    {
        public UpdateAdvertValidator()
        {
            RuleFor(x => x.Title).MinimumLength(10).When(x => !string.IsNullOrEmpty(x.Title));
            RuleFor(x => x.Description).MinimumLength(30).When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}