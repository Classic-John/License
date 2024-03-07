using Datalayer.Models;
using Datalayer.Models.Email;
using Datalayer.Models.Repositories;
using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Datalayer
{
    public class RelocationDbContext : DbContext
    {
        public RelocationDbContext(DbContextOptions<RelocationDbContext> options)
        : base(options)
        {
        }
       /* protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
           
        }
       */
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Furniture> Furnitures { get; set;}
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<IndustryUser> IndustryUsers { get; set; }
        public DbSet<SchoolUser> SchoolsUser { get; set;}
        public DbSet<Email> Emails { get; set; }
        

    }
}
