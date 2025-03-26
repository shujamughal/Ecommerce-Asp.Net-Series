using System.ComponentModel.DataAnnotations;

namespace Project_Based_Learning.Models
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password) || password.Length < 6 || !password.Any(char.IsUpper))
            {
                return new ValidationResult("Password must be at least 6 characters long and contain an uppercase letter.");
            }
            return ValidationResult.Success;
        }
    }

}
