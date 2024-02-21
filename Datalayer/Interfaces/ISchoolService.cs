using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface ISchoolService
    {
        public SchoolUser? FindSchoolUser(int id);
        public List<School>? GetSchoolServices(int id);
    }
}
