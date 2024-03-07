using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class UserRepo : BaseRepo<User>
    {
        public UserRepo(RelocationDbContext context) : base(context)
        {

        }
        public async Task<User> UpdateUser(User? user)
        {
            await Update(user);
            User? user1 = _items.Find(item => item.Id == user.Id);
            user1 = user;
            return user;
        }
    }
}
