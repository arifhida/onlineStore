using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Validations
{
    public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
    {
        public CategoryViewModelValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name cannot be empty")
                .Length(10, 256).WithMessage("minimum 8 character and maximum 20 character");
            RuleFor(x => x.CategoryDescription).Length(0, 256).
                WithMessage("Description max 256 character");
        }
    }
}
