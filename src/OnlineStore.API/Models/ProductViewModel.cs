using OnlineStore.API.Models.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models
{
    public class ProductViewModel : IValidatableObject
    {
        public long Id { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Condition { get; set; }
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Weight { get; set; }
        public bool isAvailable { get; set; }
        public List<ProductImageViewModel> Image { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ProductViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
