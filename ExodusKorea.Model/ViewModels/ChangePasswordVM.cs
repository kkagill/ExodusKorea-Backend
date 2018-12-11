using ExodusKorea.Model.ViewModels.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ExodusKorea.Model.ViewModels
{    
    public class ChangePasswordVM : IValidatableObject
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ChangePasswordVMValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}