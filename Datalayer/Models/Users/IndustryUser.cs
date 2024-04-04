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
    public class IndustryUser : BaseEntity
    {
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public string? CompanyName { get; set; }
        public int? ServiceType { get; set; }
    }
}
