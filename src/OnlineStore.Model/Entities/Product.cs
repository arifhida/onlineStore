using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class Product : EntityBase
    {
        public Product()
        {
            Image = new List<ProductImage>();
            OrderDetails = new List<OrderDetail>();
        }
        public long StoreId { get; set; }
        public Store Store { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }        
        public decimal UnitPrice { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public Condition Condition { get; set; }
        public long BrandId { get; set; }
        public Brand Brand { get; set; }
        public decimal Weight { get; set; }
        public ICollection<ProductImage> Image { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
