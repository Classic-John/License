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
        public List<User>? GetUsers()
            => _unitOfWork.Users.GetItems();
        public bool IsEmpty()
            => _unitOfWork.Users == null;
        public async Task<User> AddUser(User user)
        {
            await _unitOfWork.Users.Add(user);
            await _unitOfWork.Emails.Add(new() { UserId = GetUsers().Last().Id, Title = "Welcome to reallocation platorm", Body = $"Dear {user.Name},\n,we welcome you here and hope you will find what you need", Date = DateTime.Now, CreatorId = -1 });

            return user;
        }
        public async void RemoveUser(User user)
        {
            await _unitOfWork.Emails.DeleteAll(_unitOfWork.Emails.GetItems().Where(email => email.UserId == user.Id).ToList());
            await _unitOfWork.Users.Delete(user);
        }
        public async Task<Email?> AddEmail(Email email)
        {
            email.Date = DateTime.Now;
            await _unitOfWork.Emails.Add(email);
            return email;
        }
        public Email? FindEmail(int emailId)
            => _unitOfWork.Emails.GetItems().FirstOrDefault(email => email.Id == emailId);
        public List<Email> GetEmails(int userId)
             => _unitOfWork.Emails.GetItems().Where(email => email.UserId == userId).ToList();
        public List<Email> GetShorterEmails(int userId)
            => GetEmails(userId).Select(email => email.ShorterEmail()).ToList();

        public async Task<bool> DeleteEmail(int mailId)
        {
            try
            {
                Email? mail = _unitOfWork.Emails.GetItems().FirstOrDefault(email => email.Id == mailId);
                await _unitOfWork.Emails.Delete(mail);
            }
            catch (Exception ex) { Console.WriteLine(ex.InnerException?.Message); return false; }
            return true;
        }
        public async Task<Email> ModifyEmail(int emailId, string newBody)
        {
            Email? email = FindEmail(emailId);
            email.Body += newBody;
            email.Date = DateTime.Now;
            await _unitOfWork.Emails.Update(email);
            return email;
        }
        public User? FindUserById(int userId)
            => GetUsers().FirstOrDefault(user => user.Id == userId);
        public async Task<User> UpdateUser(int id, string? name, string? email, long? phone, string? gender, string? description, byte[]? newImage)
        {
            User? user = FindUserById(id);
            user.Name = name;
            user.Email = email;
            user.Phone = phone;
            user.Gender = gender.Equals("Male") ? 1 : 2;
            user.SelfDescription = description;
            user.ImageData = newImage;
            return await _unitOfWork.Users.Update(user);
        }
        public User? FindUserByName(string? name)
            => GetUsers().FirstOrDefault(item => item.Name.Equals(name));

        public async Task<bool> DeleteUser(int id)
        {
            User user = FindUserById(id);
            await _unitOfWork.Emails.DeleteAll(GetEmails(id));
            await _unitOfWork.Users.Delete(user);
            return true;
        }
        public User? FindUserByGoogleId(string? googleId)
            => GetUsers().FirstOrDefault(user => user.GoogleId.Equals(googleId));
    }
}
