using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Models
{
    public class FieldHasWhiteSpaceAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value.ToString();
            if (str.IndexOf(' ')==-1)
            {
                return new ValidationResult("该值必须包含空格");
            }
            return ValidationResult.Success;
        }
    }
}
