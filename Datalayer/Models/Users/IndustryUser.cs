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
    public class IndustryUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; } = default;
        public int? UserId { get; set; }
        public string? CompanyName { get; set; }
        public int? ServiceType { get; set; }
    }
}
