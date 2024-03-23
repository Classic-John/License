using Datalayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer.Models.BaseClass;

namespace Datalayer.Models.Users
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public long? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte[]? ImageData { get; set; }
        public int? Gender { get; set; }
        public string? SelfDescription { get; set; }
        public string? GoogleId { get; set; }
    }
    public static class UserExtensions
    {
        public static string? GenderString(this User user)
            => user.Gender == 1 ? "Male" : user.Gender == 2 ? "Female" : "Other";
    }
}