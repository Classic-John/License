using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IIndustryUserService
    {
        public List<IndustryUser> GetIndustryUsers();
        public void AddIndustryUser(IndustryUser industryUser);
        public void RemoveIndustryUser(IndustryUser industryUser);
    }
}
