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
        public List<User>? GetUsers();
        public Task<User> AddUser(User user);
        public void RemoveUser(User user);
        public Task<Email?> AddEmail(Email email);
        public Email? FindEmail(int emailId);
        public List<Email> GetEmails(int userId);
        public List<Email> GetShorterEmails(int userId);
        public Task<bool> DeleteEmail(int emailId);
        public Task<Email> ModifyEmail(int emailId, string newBody);
        public User? FindUserById(int userId);
        public Task<User> UpdateUser(int id, string? name, string? email, long? phone, string? gender, string? description, byte[]? newImage);
        public bool IsEmpty();
        public User? FindUserByName(string? name);
        public Task<bool> DeleteUser(int id);
        public User? FindUserByGoogleId(string? googleId);
        public List<Email> GetNumberOfNewEmails(int id);
    }
}
