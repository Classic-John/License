using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer.Models.Email;
namespace Datalayer.Models.Repositories
{
    public class EmailRepo : BaseRepo<Datalayer.Models.Email.Email>
    {
        public EmailRepo(RelocationDbContext context) : base(context)
        {

        }
        public async Task<Email.Email> UpdateEmail(Email.Email? mail)
        {
            await Update(mail);
            Email.Email? email = _items.Find(item => item.Id == mail.Id);
            email = mail;
            return mail;
        }
    }
}
