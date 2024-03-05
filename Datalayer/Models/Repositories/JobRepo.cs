using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class JobRepo:BaseRepo<Job>
    {
        public JobRepo(RelocationDbContext context):base(context)
        {
            
        }
    }
}
