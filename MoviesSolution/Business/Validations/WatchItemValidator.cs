using Business.Dtos;
using FluentValidation;

namespace Business.Validations
{
    public class WatchItemValidator : AbstractValidator<WatchItemDto>
    {
        public WatchItemValidator()
        {
            RuleFor(w => w)
                .NotNull();

            RuleFor(w => w.MovieId)
                .NotEmpty();

            RuleFor(w => w.UserId)
                .NotEmpty();
        }
    }
}
