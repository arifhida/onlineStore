using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class Brand : EntityBase
    {
        public Brand()
        {
            Product = new List<Product>();
        }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
