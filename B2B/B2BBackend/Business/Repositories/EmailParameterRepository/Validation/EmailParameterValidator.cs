using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.EmailParameterRepository.Validation
{
    internal class EmailParameterValidator : AbstractValidator<EmailParameter>
    {
        public EmailParameterValidator()
        {
            RuleFor(p => p.Smtp).NotEmpty().WithMessage("SMTP address cannot be empty!");
            RuleFor(p => p.Email).NotEmpty().WithMessage("E-mail address cannot be empty!");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password address cannot be empty!");
            RuleFor(p => p.Port).NotEmpty().WithMessage("Port address cannot be empty!");
        }
    }
}
