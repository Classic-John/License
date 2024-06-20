using Datalayer.Models;
using Datalayer.Models.Email;
using Datalayer.Models.Repositories;
using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using DataLayer.Models.Enums;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Runtime.Serialization;

namespace Datalayer
{
    public class UnitOfWork
    {
        public BookingRepo? Apartments { get; set; }
        public FurnitureRepo? Furnitures { get; set; }
        public JobRepo? Jobs { get; set; }
        public RentingRepo? Vehicles { get; set; }
        public TransportRepo? Transports { get; set; }
        public SchoolRepo? Schools { get; set; }
        public UserRepo? Users { get; set; }
        public EmailRepo? Emails { get; set; }
        public SchoolUserRepo? SchoolUsers { get; set; }
        public IndustryUserRepo? IndustryUsers { get; set; }
        private readonly RelocationDbContext? _relocationDbContext;
        public UnitOfWork(BookingRepo? br,FurnitureRepo? fr ,JobRepo? jr,RentingRepo? rp, TransportRepo?tr,SchoolRepo? sr, UserRepo? ur, EmailRepo? er, SchoolUserRepo? sur,IndustryUserRepo? iur, RelocationDbContext? rdc)
        {
            
            Apartments = br;
            Furnitures = fr;
            Jobs = jr;
            Vehicles = rp;
            Transports = tr;
            Schools = sr;
            Users = ur;
            Emails = er;
            SchoolUsers = sur;
            IndustryUsers = iur;
            _relocationDbContext = rdc;
            Apartments.InitialiseItems(_relocationDbContext);
            Furnitures.InitialiseItems(_relocationDbContext);
            Jobs.InitialiseItems(_relocationDbContext);
            Vehicles.InitialiseItems(_relocationDbContext);
            Transports.InitialiseItems(_relocationDbContext);
            Schools.InitialiseItems(_relocationDbContext);
            Users.InitialiseItems(_relocationDbContext);
            Emails.InitialiseItems(_relocationDbContext);
            SchoolUsers.InitialiseItems(_relocationDbContext);
            IndustryUsers.InitialiseItems(_relocationDbContext);
        }
    }

}
