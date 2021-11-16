using Business.Dtos;
using FluentValidation;

namespace Business.Validations
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user)
                .NotNull();

            RuleFor(user => user.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(user => user.Surname)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
