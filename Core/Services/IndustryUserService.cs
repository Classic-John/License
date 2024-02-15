using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer.Models.Enums;

namespace Core.Services
{
    public class IndustryUserService : IIndustryUserService
    {
        private readonly UnitOfWork _unitOfWork;
        public IndustryUserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<IndustryUser> GetIndustryUsers()
            => _unitOfWork.IndustryUsers;
        public void AddIndustryUser(IndustryUser industryUser)
            => _unitOfWork.IndustryUsers.Add(industryUser);
        public void RemoveIndustryUser(IndustryUser industryUser)
            => _unitOfWork.IndustryUsers.Remove(industryUser);
        public IndustryUser? FindIndustryUser(int userId)
            => GetIndustryUsers().FirstOrDefault(iUser => iUser.UserId == userId);
        public List<AbstractModel?> GetServiceList(int userId)
        {
            IndustryUser? user = FindIndustryUser(userId);
            List<AbstractModel?> services = new();
            switch (user.ServiceType)
            {
                case (int)ServiceTypes.Booking:
                    services = _unitOfWork.Apartments.Where(apartment => apartment.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Renting:
                    services = _unitOfWork.Vehicles.Where(vehicle => vehicle.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Job:
                    services = _unitOfWork.Jobs.Where(jobs => jobs.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Furniture:
                    services = _unitOfWork.FurnitureTransports.Where(furniture => furniture.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Transport:
                    services = _unitOfWork.Transports.Where(transport => transport.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
            }
            return services;
        }
        public AbstractModel? GetItem(int industryUserId, int itemId)
            => GetServiceList(industryUserId).FirstOrDefault(item => item.Id == itemId);
        public List<Apartment?> GetApartments(int industryUserId)
            => GetServiceList(industryUserId).Select(item => (Apartment)item).ToList();
        public List<Vehicle?> GetVehicles(int industryUserId)
            => GetServiceList(industryUserId).Select(item => (Vehicle)item).ToList();
        public List<Job?> GetJobs(int industryUserId)
            => GetServiceList(industryUserId).Select(item => (Job)item).ToList();
        public List<Furniture?> GetFurnitureTransports(int industryUserId)
            => GetServiceList(industryUserId).Select(item => (Furniture)item).ToList();
        public List<Transport?> GetTransports(int industryUserId)
            => GetServiceList(industryUserId).Select(item => (Transport)item).ToList();

    }
}
