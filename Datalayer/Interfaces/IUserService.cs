using Datalayer.Models.Email;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IUserService : IWrapper
    {
        public List<User> GetUsers();
        public User AddUser(User user);
        public void RemoveUser(User user);
        public Email? AddEmail(Email email);
        public Email? FindEmail(int emailId);
        public List<Email> GetEmails(int userId);
        public List<Email> GetShorterEmails(int userId);
        public User? FindUserByNameAndPassword(string name, string password);
        public bool DeleteEmail(int emailId);
        public Email ModifyEmail(int emailId, string newBody);
        public User? FindUserById(int userId);
        public void UpdateUser(int id, string? name, string? email, int? phone, string? gender, string? description, byte[]? newImage);
        public bool IsEmpty();
    }
}
