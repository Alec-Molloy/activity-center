using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Exam.Validations
{
    public class UniquePassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reg = new Regex(@"^(?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[!@#$%^&*+=])[A-Za-z\d,!@#$%^&*+=]{8,}$");
            if (value == null)
                return new ValidationResult("Password is required");
            if (!reg.IsMatch((string)value))
                return new ValidationResult("(>= 8 chars, 1 special, 1 lowercase, 1 uppercase, 1 num)");
            return ValidationResult.Success;
        }    
    }
}