using ExodusKorea.Model.ViewModels.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class LoginVM : IValidatableObject
    {       
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new LoginVMValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}