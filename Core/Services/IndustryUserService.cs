﻿using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer.Models.Enums;
using NPOI.OpenXmlFormats.Spreadsheet;

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
            => _unitOfWork.IndustryUsers.GetItems();
        public bool IsEmpty()
            => _unitOfWork.IndustryUsers == null;
        public async Task<IndustryUser> AddIndustryUser(IndustryUser industryUser)
            => await _unitOfWork.IndustryUsers.Add(industryUser);

        public async Task<bool> RemoveIndustryUser(IndustryUser industryUser)
            => await _unitOfWork.IndustryUsers.Delete(industryUser);
        public async Task<IndustryUser> UpdateIndustryUser(IndustryUser user)
            => await _unitOfWork.IndustryUsers.Update(user);
        public IndustryUser? FindIndustryUser(int userId)
            => GetIndustryUsers().FirstOrDefault(iUser => iUser.UserId == userId);
        public List<AbstractModel?> GetServiceList(int userId)
        {
            IndustryUser? user = FindIndustryUser(userId);
            List<AbstractModel?> services = new();
            switch (user.ServiceType)
            {
                case (int)ServiceTypes.Booking:
                    services = _unitOfWork.Apartments.GetItems().Where(apartment => apartment.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Renting:
                    services = _unitOfWork.Vehicles.GetItems().Where(vehicle => vehicle.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Job:
                    services = _unitOfWork.Jobs.GetItems().Where(jobs => jobs.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Furniture:
                    services =  _unitOfWork.Furnitures.GetItems().Where(furniture => furniture.CreatorId == user.UserId)
                        .Select(item => (AbstractModel?)item).ToList();
                    break;
                case (int)ServiceTypes.Transport:
                    services = _unitOfWork.Transports.GetItems().Where(transport => transport.CreatorId == user.UserId)
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
        public async Task<bool> DeleteIndustryUser(int id)
        {
            IndustryUser user= FindIndustryUser(id);
            switch(user.ServiceType) 
            {
                case 1: await _unitOfWork.Apartments.DeleteAll(GetApartments(id)); break;
                case 2: await _unitOfWork.Vehicles.DeleteAll(GetVehicles(id)); break;
                case 3: await _unitOfWork.Jobs.DeleteAll(GetJobs(id)); break;
                case 4: await _unitOfWork.Furnitures.DeleteAll(GetFurnitureTransports(id)); break;
                case 5: await _unitOfWork.Transports.DeleteAll(GetTransports(id)); break;
            }
            await _unitOfWork.IndustryUsers.Delete(user);
            return true;
        }
    }
}
