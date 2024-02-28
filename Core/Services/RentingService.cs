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
            => _unitOfWork.Vehicles;
        public void AddVehicle(Vehicle vehicle)
        {
            try { vehicle.Id = GetItems().Last().Id + 1; }
            catch(Exception) { vehicle.Id = 1; }
            _unitOfWork.Vehicles.Add(vehicle);
        }
        public void RemoveVehicle(Vehicle vehicle)=> _unitOfWork.Vehicles.Remove(vehicle);

        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach (var iu in _unitOfWork.IndustryUsers)
            {
                foreach (var vehicle in _unitOfWork.Vehicles)
                {
                    if (iu.Id == vehicle.CreatorId)
                    {
                        result.Add(iu);
                    }
                }
            }
            return result.Select(iu => iu.CompanyName).ToList();
        }
        List<AbstractModel> IService.GetItems() => GetItems().Select(item => (AbstractModel)item).ToList();
    }
}
