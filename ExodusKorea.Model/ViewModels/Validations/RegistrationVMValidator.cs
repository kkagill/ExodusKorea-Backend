using FluentValidation;

namespace ExodusKorea.Model.ViewModels.Validations
{
    public class RegistrationVMValidator : AbstractValidator<RegistrationVM>
    {
        public RegistrationVMValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.NickName).NotEmpty().WithMessage("NickName cannot be empty");
            RuleFor(vm => vm.ConfirmPassword).Equal(vm => vm.Password).WithMessage("Passwords do not match");
        }
    }
}