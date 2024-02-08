using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface ITransportService : IService
    {
        public new List<Transport> GetItems();
        public void AddTransport(Transport transport);
        public void RemoveTransport(Transport transport);
    }
}
