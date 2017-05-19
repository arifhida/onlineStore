using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class OrderDetail : EntityBase
    {
        public long OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
