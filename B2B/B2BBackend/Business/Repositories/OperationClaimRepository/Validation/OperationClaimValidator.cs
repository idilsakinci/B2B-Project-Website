using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.OperationClaimRepository.Validation
{
    public class OperationClaimValidator : AbstractValidator<OperationClaim>
    {
        public OperationClaimValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Authority name cannot be empty!");
        }
    }
}
