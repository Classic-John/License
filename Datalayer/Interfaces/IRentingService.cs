using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IRentingService : IService
    {
        public new List<Vehicle> GetItems();
        public void AddVehicle(Vehicle vehicle);
        public void RemoveVehicle(Vehicle vehicle);
    }
}
