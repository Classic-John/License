﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class FurnitureRepo : BaseRepo<Furniture>
    {
        public FurnitureRepo(RelocationDbContext context) : base(context)
        {
        }
    }
}
