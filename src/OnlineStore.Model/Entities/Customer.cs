using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class Customer :EntityBase
    {
        public Customer()
        {
            Orders = new List<Order>();
        }
        public long UserId { get; set; }
        public User User { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string City { get; set; }        
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
