using Datalayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Users
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; } = default;
        public string? Name { get; set; }
        public string? Role { get; set; }
        public int? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte[]? ImageData { get; set; }
        public int? Gender { get; set; }
        public string? SelfDescription {  get; set; }
    }
    public static class UserExtensions
    {
        public static string? GenderString(this User user)
            => user.Gender == 1 ? "Male" : "Female";
    }
}