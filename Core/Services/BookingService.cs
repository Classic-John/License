using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Users;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly UnitOfWork _unitOfWork;
        public BookingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Apartment> GetItems() => _unitOfWork.Apartments;
        public void AddApartment(Apartment apartment) => _unitOfWork.Apartments.Add(apartment);
        public void RemoveApartment(Apartment apartment) => _unitOfWork.Apartments.Remove(apartment);
        
        public List<string> GetCompanyNames()
        {
            List<IndustryUser> companyUsers = new();
            foreach(Apartment ap in _unitOfWork.Apartments)
            {
                foreach(IndustryUser ius in _unitOfWork.IndustryUsers)
                {
                    if(ius.Id==ap.CreatorId) 
                    {
                        companyUsers.Add(ius);
                    }
                }
            }
            return companyUsers.Select(cu => cu.CompanyName).ToList();
        }

        List<AbstractModel> IService.GetItems() => GetItems().Select(item =>(AbstractModel)item).ToList();
    }
}
