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
    public class SchoolUser : BaseEntity
    {
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public int? SchoolType { get; set; }

    }
}
