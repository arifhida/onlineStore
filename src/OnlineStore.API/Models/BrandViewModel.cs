using OnlineStore.API.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models
{
    public class BrandViewModel : IValidatableObject
    {
        public long Id { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new BrandViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
