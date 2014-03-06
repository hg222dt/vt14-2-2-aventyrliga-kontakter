using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


public static class MyValidationExtension
{
    public static bool Validate(this object instance, out ICollection<ValidationResult> validationResults)
    {
        var validationContext = new ValidationContext(instance);
        validationResults = new List<ValidationResult>();

        return Validator.TryValidateObject(instance, validationContext, validationResults, true);
    }
}