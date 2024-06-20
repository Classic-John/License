using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Email;
using Datalayer.Models.Repositories;
using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using NPOI.POIFS.Properties;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AbstractModel>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<IndustryUser>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SchoolUser>()
                 .HasOne<User>()
                 .WithMany()
                 .HasForeignKey(e => e.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Email>()
                 .HasOne<User>()
                 .WithMany()
                 .HasForeignKey(e => e.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<IndustryUser> IndustryUsers { get; set; }
        public DbSet<SchoolUser> SchoolsUser { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
    public static class DbSetExtensions
    {
        public static DbContext GetDbContext<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
            return currentDbContext.Context;
        }
    }
}
