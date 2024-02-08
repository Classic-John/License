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
        public void AddApartment(Apartment apartment);
        public void RemoveApartment(Apartment apartment);
    }
}
