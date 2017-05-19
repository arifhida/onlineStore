using OnlineStore.API.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models
{
    public class StoreViewModel : IValidatableObject
    {
        public long Id { get; set; }
        public string StoreName { get; set; }
        public string Motto { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string StoreLogo { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new StoreViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
