using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class ProductImage : EntityBase
    {
        public string ImageUrl { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
