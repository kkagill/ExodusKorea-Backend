using FluentValidation;

namespace ExodusKorea.Model.ViewModels.Validations
{
    public class ChangePasswordVMValidator : AbstractValidator<ChangePasswordVM>
    {
        public ChangePasswordVMValidator()
        {
            RuleFor(vm => vm.OldPassword).NotEmpty().WithMessage("Old Password cannot be empty");
            RuleFor(vm => vm.NewPassword).NotEmpty().WithMessage("New Password cannot be empty");
            RuleFor(vm => vm.ConfirmPassword).Equal(vm => vm.NewPassword).WithMessage("Passwords do not match");
        }
    }
}