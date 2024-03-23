using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IBookingService : IService
    {
        public new List<Apartment> GetItems();
        public Task<Apartment> AddApartment(Apartment apartment);
        public Task<bool>RemoveApartment(Apartment apartment);
        public Task<Apartment> UpdateApartment(Apartment apartment);
    }
}
