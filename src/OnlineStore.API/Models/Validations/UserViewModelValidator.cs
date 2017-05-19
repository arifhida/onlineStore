using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Validations
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(User => User.FullName).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(User => User.Email).NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Valid email address required");
            RuleFor(u => u.UserName).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(u => u.Password).NotEmpty().WithMessage("Pasword cannot be empty").Length(8, 256)
                .WithMessage("password minimum 8 character and maximum 20 character");
        }
    }
}
