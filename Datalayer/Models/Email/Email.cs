using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Email
{
    public class Email
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int? UserId { get; set; }
        public int? CreatorId { get; set; }
    }
    public static class EmailExtensions
    {
        public static Email ShorterEmail(this Email email)
            => new()
            {
                Id = email.Id,
                Title = email.Title?.Length > 20 ? email.Title.Substring(0, 19)+"..." : email.Title,
                Body = email.Body?.Length > 40 ? email.Body.Substring(0, 39)+"..." : email.Body,
                CreatorId = email.CreatorId,
                UserId = email.UserId
            };
    }
}

