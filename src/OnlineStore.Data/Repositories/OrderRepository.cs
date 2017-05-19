using OnlineStore.Data.Abstract;
using OnlineStore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data.Repositories
{
    public class OrderRepository : EntityBaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context)
        {
        }
    }
}
