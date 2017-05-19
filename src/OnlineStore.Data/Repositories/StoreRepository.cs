﻿using OnlineStore.Data.Abstract;
using OnlineStore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data.Repositories
{
    public class StoreRepository : EntityBaseRepository<Store>, IStoreRepository
    {
        public StoreRepository(DataContext context) : base(context)
        {
        }
    }
}
