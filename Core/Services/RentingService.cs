using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RentingService : IRentingService
    {
        private readonly UnitOfWork _unitOfWork;
        public RentingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Vehicle> GetItems()
            => _unitOfWork.Vehicles.GetItems();
        public async void AddVehicle(Vehicle vehicle) 
            => await _unitOfWork.Vehicles.Add(vehicle);
        public async void RemoveVehicle(Vehicle vehicle)
            => await _unitOfWork.Vehicles.Delete(vehicle);

        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach (var iu in _unitOfWork.IndustryUsers.GetItems())
            {
                foreach (var vehicle in GetItems())
                {
                    if (iu.Id == vehicle.CreatorId)
                    {
                        result.Add(iu);
                    }
                }
            }
            return result.Select(iu => iu.CompanyName).ToList();
        }
        List<AbstractModel> IService.GetItems() 
            => GetItems().Select(item => (AbstractModel)item).ToList();
    }
}
