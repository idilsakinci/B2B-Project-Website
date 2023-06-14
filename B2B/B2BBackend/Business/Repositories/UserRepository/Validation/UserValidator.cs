using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.UserRepository.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Username cannot be empty!");
            RuleFor(p => p.Email).NotEmpty().WithMessage("E-mail address cannot be empty!");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Write a valid e-mail address!");
            RuleFor(p => p.ImageUrl).NotEmpty().WithMessage("User picture cannot be empty!");
        }
    }
}
