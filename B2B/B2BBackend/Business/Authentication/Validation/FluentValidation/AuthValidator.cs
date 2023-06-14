using Entities.Dtos;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AuthValidator : AbstractValidator<RegisterAuthDto>
    {
        public AuthValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Username cannot be empty!");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Mail address cannot be empty!");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Type a valid email address!");
            RuleFor(p => p.Image.FileName).NotEmpty().WithMessage("User image cannot be empty!");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password cannot be empty!");
            //RuleFor(p => p.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters!");
            //RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Your password must contain at least 1 uppercase letter!");
            //RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Your password must contain at least 1 lowercase letter!");
            //RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Your password must contain at least 1 number");
            //RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Your password must contain at least 1 special character!");
        }
    }
}
