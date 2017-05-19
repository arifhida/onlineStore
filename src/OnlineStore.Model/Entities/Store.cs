using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class Store : EntityBase
    {
        public Store()
        {
            Products = new List<Product>();
        }
        public string StoreName { get; set; }
        public string Motto { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string StoreLogo { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
