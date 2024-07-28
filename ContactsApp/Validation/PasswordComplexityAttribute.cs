﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ContactsApp.Validation
{
        //klasa odpowiedzialna za walidację poprawności tworzonego hasła
        //hasło musi być dłuższe niż 7 znaków
        //zawierać min. 1 małą i wielką literę,
        //cyfrę oraz znak specjalny
        public class PasswordComplexityAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult("Password is required.");
                }

                var password = value.ToString();

                if (password.Length < 8)
                {
                    return new ValidationResult("Password must be at least 8 characters long.");
                }

                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    return new ValidationResult("Password must contain at least one uppercase letter.");
                }

                if (!Regex.IsMatch(password, @"[a-z]"))
                {
                    return new ValidationResult("Password must contain at least one lowercase letter.");
                }

                if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    return new ValidationResult("Password must contain at least one digit.");
                }

                if (!Regex.IsMatch(password, @"[\W_]"))
                {
                    return new ValidationResult("Password must contain at least one special character.");
                }

                return ValidationResult.Success;
            }
        }
}
