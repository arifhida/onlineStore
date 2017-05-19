using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Validations
{
    public class StoreViewModelValidator : AbstractValidator<StoreViewModel>
    {
        public StoreViewModelValidator()
        {
            RuleFor(Store => Store.StoreName).NotEmpty().WithMessage("Store name cannot be empty")
                .MaximumLength(150).WithMessage("Maximum length 150 character");
            RuleFor(Store => Store.Motto).MaximumLength(256).WithMessage("Maximum length 256 character");
            RuleFor(Store => Store.Description).MaximumLength(550).WithMessage("Maximum length 550 character");
            RuleFor(Store => Store.Address).MaximumLength(550).WithMessage("Maximum length 550 character");
            RuleFor(Store => Store.PostalCode).MaximumLength(10).WithMessage("Maximum length 10 character");
        }
    }
}
