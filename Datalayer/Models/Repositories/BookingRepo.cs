using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class BookingRepo:BaseRepo<Apartment>
    {
        public BookingRepo(RelocationDbContext context) : base(context)
        {
        }
        public async Task<Apartment?> UpdateApartment(Apartment? apartment)
        {
           await Update(apartment);
            Apartment? apartment1 = _items.Find(item => item.Id == apartment.Id);
            apartment1 = apartment;
            return apartment1;
        }
       
    }
}
