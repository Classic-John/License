
using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IService
    {
        public List<string> GetCompanyNames();
        public List<AbstractModel> GetItems();
    }
}
