using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class Role : EntityBase
    {
        public Role()
        {
            UserInRole = new List<UserInRole>();
        }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public ICollection<UserInRole> UserInRole { get; set; }
    }
}
