using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.UserOperationClaimRepository.Validation
{
    public class UserOperationClaimValidator : AbstractValidator<UserOperationClaim>
    {
        public UserOperationClaimValidator()
        {
            RuleFor(p => p.UserId).Must(IsIdValid).WithMessage("You must select a user for authorization assignment!");
            RuleFor(p => p.OperationClaimId).Must(IsIdValid).WithMessage("For authorization assignment, you must select authorization!");
        }

        private bool IsIdValid(int id)
        {
            if (id > 0 && id != null)
            {
                return true;
            }
            return false;
        }
    }
}
