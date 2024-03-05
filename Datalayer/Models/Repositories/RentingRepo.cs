using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class RentingRepo:BaseRepo<Vehicle>
    {
        public RentingRepo(RelocationDbContext context) :base(context)
        {
            
        }
    }
}
