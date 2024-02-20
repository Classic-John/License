using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IIndustryUserService
    {
        public List<IndustryUser> GetIndustryUsers();
        public void AddIndustryUser(IndustryUser industryUser);
        public void RemoveIndustryUser(IndustryUser industryUser);
        public IndustryUser? FindIndustryUser(int industryUserId);
        public List<Apartment?> GetApartments(int industryUserId);
        public List<Vehicle?> GetVehicles(int industryUserId);
        public List<Job?> GetJobs(int industryUserId);
        public List<Furniture?> GetFurnitureTransports(int industryUserId);
        public List<Transport?> GetTransports(int industryUserId);
        public List<AbstractModel?> GetServiceList(int industryUserId);
        public AbstractModel? GetItem(int industryUserId, int itemId);
        public void ModifyCompanyNameOnOffers(int id, string? companyName);

    }
}
