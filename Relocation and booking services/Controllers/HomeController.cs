using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Datalayer.Interfaces;
using DataLayer.Models.Enums;
using Datalayer.Models.Wrapper;
using Relocation_and_booking_services.Pages.User;
using Datalayer.Models.Users;
using Microsoft.AspNetCore.Mvc.Filters;
using Bogus.DataSets;
using Bogus;
using System.Reflection.Metadata.Ecma335;
using Datalayer.Models.Enums;

namespace Relocation_and_booking_services.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public static bool logged = false;
        public static User CurrentUser { get; set; }
        public HomeController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
        }
        public static int GetCurrentRole()
            => CurrentUser == null ? 0 : CurrentUser.Role.Equals("User") ? 1 : CurrentUser.Role.Equals("IndustryUser") ? 2 : 3;
        private IActionResult ViewSelection(int role)
        {
            ViewBag.Role = role;
            switch (role)
            {
                case (int)Roles.User: return RedirectToAction("UserHome", "User");
                case (int)Roles.IndustryUser: return RedirectToAction("IndustryUserHome", "IndustryUser");
                case (int)Roles.SchoolUser: return RedirectToAction("SchoolServices", "School");
                default: break;
            }
            return View("Homepage");
        }
        [Route("/")]
        [Route("Homepage")]
        public IActionResult Homepage()
        {
            ViewBag.Role = CurrentUser == null ? 0 : GetCurrentRole();
            return View("Homepage");
        }

        //FINISH LOG IN ASWELL
        [Route("Log in")]
        public IActionResult LogIn()
        {
            string name = Request.Form["Name"];
            string password = Request.Form["Password"];
            CurrentUser = _serviceWrapper._userService.FindUserByNameAndPassword(name, password);
            ViewBag.Role = GetCurrentRole();
            if (CurrentUser == null)
                return View("Failed");
            int userType = 0;
            foreach (Roles item in Enum.GetValues(typeof(Roles)))
            {
                if (Enum.GetName(item).Equals(CurrentUser.Role))
                {
                    userType = (int)item;
                    break;
                }
            }
            return ViewSelection(userType);
        }
        [Route("Sign out")]
        public IActionResult Logout()
        {
            CurrentUser = null;
            ViewBag.Role = 0;
            return View("Homepage");
        }
        [Route("KeepPicture")]
        public string KeepPicture()
            => CurrentUser == null ? "/default.jpg" : CurrentUser.ImageData == null ? "/default.jpg" : ("data:image/jpg;base64," + Convert.ToBase64String(CurrentUser.ImageData));
        public static async Task<byte[]?> ConvertImageToBytes(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
            return null;
        }

        //Finish Create Account
        [Route("Create Account")]
        public IActionResult CreateAccount()
        {
            int role = 0;
            int? id = -1;
            role = Convert.ToInt32(Request.Form["Option"]);
            GlobalService.Role = role;
            try { id = _serviceWrapper._userService.GetUsers().Last().Id + 1; }
            catch (Exception) { id = 1; }
            CurrentUser = _serviceWrapper._userService.AddUser(new()
            {
                Id = id,
                Name = Request.Form["Name"],
                Role = GlobalService.roleNames[role],
                Gender = Convert.ToInt32(Request.Form["gender"]),
                Email = Request.Form["Email"].ToString(),
                Phone = Convert.ToInt32(Request.Form["Phone"].ToString()),
                Password = Request.Form["password"],
                ImageData = ConvertImageToBytes(Request.Form.Files["photo"]).Result
            });
            try { id = _serviceWrapper._industryUserService.GetIndustryUsers().Last().Id + 1; }
            catch (Exception) { id = 1; }
            if (role == (int)Roles.IndustryUser)
                _serviceWrapper._industryUserService.AddIndustryUser(new()
                {
                    Id = id,
                    Name = Request.Form["Name"],
                    Email = Request.Form["Email"].ToString(),
                    Phone = Convert.ToInt32(Request.Form["Phone"].ToString()),
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id,
                    CompanyName = Request.Form["Company"].ToString(),
                    ServiceType = Convert.ToInt32(Request.Form["serviceType"])
                });
            try { id = _serviceWrapper._schoolService.GetSchoolUsers().Last().Id + 1; }
            catch (Exception) { id = 1; };
            if (role == (int)Roles.SchoolUser)
            {
                _serviceWrapper._schoolService.AddSchoolUser(new()
                {
                    Id =id,
                    SchoolType = Convert.ToInt32(Request.Form["schoolType"]),
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id
                });
            }
            return ViewSelection(role);
        }
        [Route("About Us")]
        public IActionResult AboutUs()
        {
            ViewBag.Role = GetCurrentRole();
            return View("AboutUs");
        }

        [Route("Booking Services")]
        public IActionResult AllBookings()
        {
            ViewBag.Role = GetCurrentRole();
            return View("BookingServices", _serviceWrapper._bookingService.GetItems());
        }

        [Route("Renting Services")]
        public IActionResult AllRentings()
        {
            ViewBag.Role = GetCurrentRole();
            return View("RentingServices", _serviceWrapper._rentingService.GetItems());
        }

        [Route("Furniture Transports Services")]
        public IActionResult AllFurnitureTransports()
        {
            ViewBag.Role = GetCurrentRole();
            return View("AllFurnitureTransports", _serviceWrapper._furnitureService.GetItems());
        }

        [Route("Jobs Services")]
        public IActionResult AllJobs()
        {
            ViewBag.Role = GetCurrentRole();
            return View("AllJobs", _serviceWrapper._jobService.GetItems());
        }

        [Route("All Transports")]
        public IActionResult AllTransports()
        {
            ViewBag.Role = GetCurrentRole();
            return View("AllTransports", _serviceWrapper._transportService.GetItems());
        }
        [Route("Profile")]
        public IActionResult Profile()
        {
            ViewBag.Role = GetCurrentRole();
            if (CurrentUser == null)
                return View("Failed");
            User user = _serviceWrapper._userService.FindUserById(CurrentUser.Id.Value);
            IndustryUser? industryUser = _serviceWrapper._industryUserService.FindIndustryUser(CurrentUser.Id.Value);
            return View("CurrentProfile", (user, industryUser));
        }
        public IActionResult ModifyProfile()
        {
            ViewBag.Role = GetCurrentRole();
            string? name = Request.Form["name"];
            string? email = Request.Form["email"];
            int? phone = Convert.ToInt32(Request.Form["phone"]);
            string? gender = Request.Form["gender"];
            string? description = Request.Form["description"];
            string? companyName = Request.Form["companyName"];
            int? serviceType;
            foreach (int service in Enum.GetValues(typeof(ServiceTypes)))
                if (nameof(service).Equals(Request.Form["serviceType"]))
                {
                    serviceType = service;
                    break;
                }
            byte[]? newImage = ConvertImageToBytes(Request.Form.Files["newPhoto"]).Result;
            _serviceWrapper._userService.UpdateUser(CurrentUser.Id.Value, name, email, phone, gender, description, newImage);
            if (!CurrentUser.Role.Equals("User"))
            {
                _serviceWrapper._industryUserService.FindIndustryUser(CurrentUser.Id.Value).CompanyName = companyName;
                _serviceWrapper._industryUserService.ModifyCompanyNameOnOffers(CurrentUser.Id.Value, companyName);
            }
            return View("Homepage");
        }

    }
}
