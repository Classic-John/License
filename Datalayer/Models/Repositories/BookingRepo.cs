﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class BookingRepo:BaseRepo<Apartment>
    {
        public BookingRepo(RelocationDbContext context) : base(context)
        {
        }
       
    }
}
