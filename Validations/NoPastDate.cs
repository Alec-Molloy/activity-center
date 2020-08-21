using System;
using System.ComponentModel.DataAnnotations;

namespace Exam.Validations
{
    public class NoPastDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is DateTime)
            {
                DateTime check = (DateTime)value;
                if(check.Date < DateTime.Now.Date)
                {
                    return new ValidationResult("Date must be in the future");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("Please enter a valid date");
            }
        }
    }
}