using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Validations
{
    public class BrandViewModelValidator : AbstractValidator<BrandViewModel>
    {
        public BrandViewModelValidator()
        {
            RuleFor(x => x.BrandName).MaximumLength(250).WithMessage("Maximum length 250").NotEmpty()
                .WithMessage("Brand Name cannot be empty");
            RuleFor(x => x.Description).MaximumLength(550).WithMessage("Maximum length 550");            
        }
    }
}
