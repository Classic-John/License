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
        public Task<Transport> AddTransport(Transport transport);
        public Task<bool> RemoveTransport(Transport transport);
        public Task<Transport> UpdateTransport(Transport transport);
    }
}
