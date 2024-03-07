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
        public List<Apartment> GetItems()
            => _unitOfWork.Apartments.GetItems();
        public async void AddApartment(Apartment apartment)
            => await _unitOfWork.Apartments.Add(apartment);

        public async void RemoveApartment(Apartment apartment)
            => await _unitOfWork.Apartments.Delete(apartment);

        public List<string> GetCompanyNames()
        {
            List<IndustryUser> companyUsers = new();
            foreach (Apartment ap in GetItems())
            {
                foreach (IndustryUser ius in _unitOfWork.IndustryUsers.GetItems())
                {
                    if (ius.Id == ap.CreatorId)
                    {
                        companyUsers.Add(ius);
                    }
                }
            }
            return companyUsers.Select(cu => cu.CompanyName).ToList();
        }

        List<AbstractModel> IService.GetItems()
            => GetItems().Select(item => (AbstractModel)item).ToList();

    }
}
