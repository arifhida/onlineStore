using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Validations
{
    public class ProductViewModelValidator: AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator()
        {
            RuleFor(x => x.SKU).NotEmpty().WithMessage("SKU cannot be empty");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product Name cannot be empty");
            RuleFor(x => x.ProductDescription).NotEmpty().WithMessage("Description cannot be empty");
        }
    }
}
