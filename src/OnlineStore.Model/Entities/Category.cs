using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Model.Entities
{
    public class Category :EntityBase
    {
        public Category()
        {
            SubCategory = new List<Category>();
            Product = new List<Product>();
        }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string Logo { get; set; }
        public long? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> SubCategory { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
