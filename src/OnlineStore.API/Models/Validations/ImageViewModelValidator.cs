using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Validations
{
    public class ImageViewModelValidator : AbstractValidator<ProductImageViewModel>
    {
        public ImageViewModelValidator()
        {
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Url cannot be empty");
        }
    }
}
