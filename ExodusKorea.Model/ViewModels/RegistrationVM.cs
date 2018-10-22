using ExodusKorea.Model.ViewModels.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class RegistrationVM : IValidatableObject
    {               
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new RegistrationVMValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}