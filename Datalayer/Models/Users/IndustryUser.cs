using Datalayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Users
{
    public class IndustryUser
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? UserId {  get; set; }
        public string? CompanyName {  get; set; }
        public int? Phone {  get; set; }
        public string? Email { get; set; }
        public int? ServiceType { get; set; }
    }
}
