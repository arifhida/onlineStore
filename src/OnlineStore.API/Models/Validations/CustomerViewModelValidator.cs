using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Validations
{
    public class CustomerViewModelValidator : AbstractValidator<CustomerViewModel>
    {
        public CustomerViewModelValidator()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address cannot be empty");
            RuleFor(x => x.City).NotEmpty().WithMessage("City cannot be empty");
            RuleFor(x => x.District).NotEmpty().WithMessage("District cannot be empty");
            RuleFor(x => x.Province).NotEmpty().WithMessage("Province cannot be empty");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone Number cannot be empty");            
        }
    }
}
