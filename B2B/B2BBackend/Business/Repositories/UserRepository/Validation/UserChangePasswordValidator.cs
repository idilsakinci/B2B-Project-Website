using Entities.Dtos;
using FluentValidation;

namespace Business.Repositories.UserRepository.Validation
{
    public class UserChangePasswordValidator : AbstractValidator<UserChangePasswordDto>
    {
        public UserChangePasswordValidator()
        {
            RuleFor(p => p.NewPassword).NotEmpty().WithMessage("Password cannot be empty!");
            RuleFor(p => p.NewPassword).MinimumLength(6).WithMessage("Password must be at least 6 characters!");
            RuleFor(p => p.NewPassword).Matches("[A-Z]").WithMessage("Your password must contain at least 1 uppercase letter!");
            RuleFor(p => p.NewPassword).Matches("[a-z]").WithMessage("Your password must contain at least 1 lowercase letter!");
            RuleFor(p => p.NewPassword).Matches("[0-9]").WithMessage("Your password must contain at least 1 number!");
            RuleFor(p => p.NewPassword).Matches("[^a-zA-Z0-9]").WithMessage("Your password must contain at least 1 special character!");
        }
    }
}
