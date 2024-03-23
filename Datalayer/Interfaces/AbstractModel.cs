using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer.Models.BaseClass;
namespace Datalayer.Interfaces
{
    public abstract class AbstractModel:BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CreatorId { get; set; }
        public int? Price { get; set; }
        public string? Link { get; set; }
        public string? Location { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? Date { get; set; }
    }
}
