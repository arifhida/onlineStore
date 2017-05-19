using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class UserInRole : EntityBase
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long RoleId { get; set; }
        public Role Role { get; set; }
    }
}
