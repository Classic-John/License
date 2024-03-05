using Datalayer.Models;
using Datalayer.Models.Email;
using Datalayer.Models.Repositories;
using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using DataLayer.Models.Enums;

namespace Datalayer
{
    //Pune listele in repozitoarele lor si acceseazale de acolo+ SCAPA DE "ImageSrc" SI DE "CompanyName" DIN ULTIMELE 5 MODELE
    public class UnitOfWork
    {
        public List<BookingRepo> Apartments1;
        public List<FurnitureRepo> Furnitures1;
        public List<JobRepo> Jobs1;
        public List<RentingRepo> Vehicles1;
        public List<TransportRepo> Transport1;
        public List<SchoolRepo> Schools1;
        public List<UserRepo> Users1;
        public List<EmailRepo> Emails1;
        public List<SchoolUserRepo> SchoolUsers1;
        public List<IndustryUserRepo> IndustryUsers1;

        public List<Apartment> Apartments { get; set; } = new();
        public List<Furniture> FurnitureTransports { get; set; } = new();
        public List<Job> Jobs { get; set; } = new();
        public List<Vehicle> Vehicles { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public List<IndustryUser> IndustryUsers { get; set; } = new();
        public List<Transport> Transports { get; set; } = new();
        public List<Email> Emails { get; set; } = new(); 
        public List<School> Schools { get; set; } = new();
        public List<SchoolUser> SchoolUsers { get; set; } = new();

        //Scapa de metoda asta odata
        public UnitOfWork()
        {
           // temporaryInitialise();
        }
        private void temporaryInitialise()
        {
            Apartments = new()
            {
                new(){ CreatorId=2, Description="Beautiful and cheap apartment", Id=1, ImageSrc="https://images.pexels.com/photos/1571460/pexels-photo-1571460.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", Name="Brasov apartment", Price=1000, Link="", CompanyName="Rolemus", Location="Brasov"},
                new(){ CreatorId=3, Description="This is an expensive apartment", Id=2, ImageSrc="https://images.pexels.com/photos/2121120/pexels-photo-2121120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1/", Name="Bucharest apartment", Price=3000,Link = "", CompanyName="Siemens", Location="Bucharest"},
            };
            FurnitureTransports = new()
            {
                new(){ CreatorId=2, Description="Cheap furniture transport", Id=1, ImageSrc="https://st5.depositphotos.com/1010613/67314/i/1600/depositphotos_673147618-stock-photo-truck-movers-loading-van-carrying.jpg", Name="Brasov furniture transport", Price=100, Link = "https://www.premiermoving.ro/en", CompanyName= "Rolemus", Location = "Brasov"},
                new(){ CreatorId=3, Description="This is an expensive furniture transport", Id=2, ImageSrc="https://st3.depositphotos.com/1010613/18046/i/1600/depositphotos_180462184-stock-photo-two-young-male-movers-uniform.jpg/", Name="Bucharest furniture transport", Price=1500, Link = "https://relokat.ro/en ", CompanyName= "Siemens", Location="Bucharest"},
            };
            Jobs = new()
            {
                new(){ CreatorId=2, Description="Cool and easy job", Id=1, ImageSrc="https://media.evz.ro/wp-content/uploads/2012/04/gunoier-meserie-de-aur-15-candidati-pe-un-post-mai-tare-ca-la-me.jpg", Name="Bucharest job offer", Price=1000, Link = "https://www.publi24.ro/anunturi/locuri-de-munca/salubrizare-curatenie-dezinsectie/anunt/angajare-personal-curatenie-scari-de-bloc-si-subsol-bucuresti-program-lucru-8h-zilnic/16f74hi9006h7i3feh0205g2321f8g0e.html", CompanyName="Rolemus", Location="Bucharest"},
                new(){ CreatorId=3, Description="Well payed job", Id=2, ImageSrc="https://media.hotnews.ro/media_server1/image-2020-06-9-24045732-41-programator.jpg", Name="Brasov Job Offer", Price=3000, Link = "https://www.ejobs.ro/user/locuri-de-munca/junior-software-developer-erp-software/1734603", CompanyName="Siemens", Location = "Brasov"},
            };
            Vehicles = new()
            {
                new(){ CreatorId=2, Description="Beautiful and expensive vehicle", Id=1, ImageSrc="https://wallpapercave.com/wp/2l37i4C.jpg", Name="Brasov car", Price=99999, Link="https://www.momondo.ro/inchirieri-auto", CompanyName="Rolemus", Location="Brasov"},
                new(){ CreatorId=3, Description="This is an cheap van", Id=2, ImageSrc="https://media2.lajumate.ro/media/i/cart/5/125/12533305_renault-trafic19-diesel2006acfinantare-rate_8.jpg", Name="Bucharest van", Price=1000,Link = "https://www.rent-a-duba.ro/", CompanyName="Siemens", Location="Bucharest"},
            };
            Transports = new()
            {
                new(){ CreatorId=2, Description="Cheap travel", Id=1, ImageSrc="https://scontent.fsbz3-1.fna.fbcdn.net/v/t39.30808-6/347380612_3378228152419072_7647320787394836772_n.png?ccb=1-7&_nc_sid=efb6e6&_nc_ohc=DB9LNXexqQ0AX8sXvL_&_nc_ht=scontent.fsbz3-1.fna&oh=00_AfBVLOqGPTGRePtYU4cKjviu27UarJ4b5ZlPZ1BFq4g_Cw&oe=65AE847F", Name="Brasov car", Price=100, Link="https://gtvbus.ro/o", CompanyName="Rolemus", Location="Brasov"},
                new(){ CreatorId=3, Description="Expensive travel", Id=2, ImageSrc="https://scontent.fsbz3-1.fna.fbcdn.net/v/t39.30808-6/393409316_635211942112397_2548563276938212288_n.jpg?_nc_cat=110&ccb=1-7&_nc_sid=efb6e6&_nc_ohc=L96YY5zI7W8AX96_lu9&_nc_ht=scontent.fsbz3-1.fna&oh=00_AfAecj1e_Oq8zT-CZzGnm1wJhZuCVCcg3x3BmRTCn4LEJw&oe=65ADFCC8", Name="Bucharest van", Price=1000,Link = "https://www.autogari.ro/Transport/Iasi-ClujNapoca", CompanyName="Siemens", Location="Bucharest"},
            };
            Users = new()
            {
                new(){ Id=1, Name="Jonny Cage", Role="User", Email="johhny@cage.com", Password="jonnyIsCool", Phone=0743666999},
                new() { Id=2, Name="Bill Gates", Role="IndustryUser", Email="gates@money.com", Password="moneymoney",Phone=0799111222},
                new(){Id=3, Name="Ramona Montana", Role= "IndustryUser", Email="idk@yikes.com", Password="hannah", Phone=0733222444},
            };
            IndustryUsers = new()
            {
                new(){Id=1, UserId=2, CompanyName="Rolemus", ServiceType=1},
                new(){Id=2, UserId=3,CompanyName="Siemens", ServiceType=2}
            };
            Emails = new()
            {
                new(){Id=1, Body="I likes Tacos", Title="Food tastes", UserId=1, CreatorId=3},
                new(){Id=2, Title="Test email", Body="This new email system might work",UserId=2, CreatorId=3},
                new(){Id=3,Title="Another email,but about money", Body="I forgot to mention that i love the smell the money in the morning", UserId=3, CreatorId=2},
                new(){Id=4, Title="This tool might be have a use", Body="I have discovered how to send an email to you", UserId=3, CreatorId=2},
                new(){Id=5,Title="Cool email bro", Body="Now i can bother you whenever i want", UserId=1, CreatorId=3},
            };
            Schools = new();
            SchoolUsers = new();
        }
    }
}
