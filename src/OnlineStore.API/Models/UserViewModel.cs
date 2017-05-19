using OnlineStore.API.Models.Validations;
using OnlineStore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models
{
    public class UserViewModel : IValidatableObject
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Photo { get; set; }
        public List<UserInRoleViewModel> Roles { get; set; }
        public StoreViewModel Store { get; set; }
        public string Confirmation { get; set; }
        public bool isConfirmed { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new UserViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
