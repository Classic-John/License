using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IIndustryUserService : IWrapper
    {
        public List<IndustryUser> GetIndustryUsers();
        public Task<IndustryUser> AddIndustryUser(IndustryUser industryUser);
        public Task<bool> RemoveIndustryUser(IndustryUser industryUser);
        public Task<IndustryUser> UpdateIndustryUser(IndustryUser user);
        public IndustryUser? FindIndustryUser(int industryUserId);
        public List<Apartment?> GetApartments(int industryUserId);
        public List<Vehicle?> GetVehicles(int industryUserId);
        public List<Job?> GetJobs(int industryUserId);
        public List<Furniture?> GetFurnitureTransports(int industryUserId);
        public List<Transport?> GetTransports(int industryUserId);
        public List<AbstractModel?> GetServiceList(int industryUserId);
        public AbstractModel? GetItem(int industryUserId, int itemId);
        public bool IsEmpty();
        public Task<bool> DeleteIndustryUser(int id);
    }
}
