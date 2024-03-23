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
        public Task<Vehicle> AddVehicle(Vehicle vehicle);
        public Task<bool> RemoveVehicle(Vehicle vehicle);
        public Task<Vehicle> UpdateVehicle(Vehicle vehicle);
    }
}
