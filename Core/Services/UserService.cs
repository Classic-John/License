using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models.Email;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<User> GetUsers()
            => _unitOfWork.Users;
        public bool IsEmpty()
            => _unitOfWork.Users == null;
        public User AddUser(User user)
        {
            int? lastId = 1;
            try { lastId = _unitOfWork.Emails.Last().Id + 1; }
            catch(Exception) { lastId = 1; }
            int? id = lastId;
            _unitOfWork.Emails.Add(new() { Id = id, UserId = user.Id, Title = "Welcome to reallocation platorm", Body = $"Dear {user.Name},\n,we welcome you here and hope you will find what you need", Date = DateTime.Now});
            _unitOfWork.Users.Add(user);
            return user;
        }
        public void RemoveUser(User user)
        {
            _unitOfWork.Emails.RemoveAll(email => email.UserId == user.Id);
            _unitOfWork.Users.Remove(user);
        }
        public Email? AddEmail(Email email)
        {
            email.Id = _unitOfWork.Emails.Count > 0 ? _unitOfWork.Emails.Last().Id + 1 : 1;
            email.Date = DateTime.Now;
            _unitOfWork.Emails.Add(email);
            return email;
        }
        public Email? FindEmail(int emailId)
            => _unitOfWork.Emails.FirstOrDefault(email => email.Id == emailId);
        public List<Email> GetEmails(int userId)
            => _unitOfWork.Emails.Where(email => email.UserId == userId).ToList();
        public List<Email> GetShorterEmails(int userId)
            => GetEmails(userId).Select(email => email.ShorterEmail()).ToList();
        public User? FindUserByNameAndPassword(string name, string password)
            => _unitOfWork.Users.FirstOrDefault(user => user.Name.Equals(name) && user.Password.Equals(password));
        public bool DeleteEmail(int mailId)
            => _unitOfWork.Emails.Remove(_unitOfWork.Emails.FirstOrDefault(email => email.Id == mailId));
        public Email ModifyEmail(int emailId, string newBody)
        {
            Email? email = FindEmail(emailId);
            email.Body += newBody;
            email.Date = DateTime.Now;
            return email;
        }
        public User? FindUserById(int userId)
            => _unitOfWork.Users.FirstOrDefault(user => user.Id == userId);
        public void UpdateUser(int id, string? name, string? email, int? phone, string? gender, string? description, byte[]? newImage)
        {
            User? user = FindUserById(id);
            user.Name = name;
            user.Email = email;
            user.Phone = phone;
            user.Gender = gender.Equals("User") ? 1 : 2;
            user.SelfDescription = description;
            user.ImageData = newImage;
        }
    }
}
