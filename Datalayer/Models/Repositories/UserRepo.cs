﻿using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class UserRepo : BaseRepo<User>
    {
        public UserRepo(RelocationDbContext context) : base(context)
        {

        }
    }
}
