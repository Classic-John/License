using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer.Models.Email;
namespace Datalayer.Models.Repositories
{
    public class EmailRepo : BaseRepo<Datalayer.Models.Email.Email>
    {
        public EmailRepo(RelocationDbContext context) : base(context)
        {

        }
    }
}
