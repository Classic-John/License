﻿using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Datalayer.Interfaces;
using DataLayer.Models.Enums;
using Datalayer.Models.Wrapper;
using Relocation_and_booking_services.Pages.User;
using Datalayer.Models.Users;
using Microsoft.AspNetCore.Mvc.Filters;
using Bogus.DataSets;
using Bogus;

namespace Relocation_and_booking_services.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public static bool logged = false;
        public static User CurrentUser { get; set; }
        public HomeController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService);
        }
        public static int GetCurrentRole()
            => CurrentUser.Role.Equals("User") ? 1 : CurrentUser.Role.Equals("IndustryUser") ? 2 : 0;
        private IActionResult ViewSelection(int role)
        {
            ViewBag.Role = role;
            switch (role)
            {
                case (int)Roles.User: return RedirectToAction("UserHome", "User");
                case (int)Roles.IndustryUser: return RedirectToAction("IndustryUserHome", "IndustryUser");
                default: break;
            }
            return View("Homepage");
        }
        [Route("/")]
        [Route("Homepage")]
        public IActionResult Homepage()
        {
            ViewBag.Role = 0;
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
        //Finish Create Account
        [Route("Create Account")]
        public IActionResult CreateAccount()
        {
            int role = 0;
            role = Convert.ToInt32(Request.Form["Option"]);
            GlobalService.Role = role;
            CurrentUser = _serviceWrapper._userService.AddUser(new()
            {
                Id = _serviceWrapper._userService.GetUsers().Last().Id+1,
                Name = Request.Form["Name"],
                Role = GlobalService.roleNames[role],
                Email = Request.Form["Email"].ToString(),
                Phone = Convert.ToInt32(Request.Form["Phone"].ToString()),
                Password = Request.Form["password"]
            });

            if (role == (int)Roles.IndustryUser)
                _serviceWrapper._industryUserService.AddIndustryUser(new()
                {
                    Id = _serviceWrapper._industryUserService.GetIndustryUsers().Last().Id+1,
                    Name = Request.Form["Name"],
                    Email = Request.Form["Email"].ToString(),
                    Phone = Convert.ToInt32(Request.Form["Phone"].ToString()),
                    UserId = 3,
                    CompanyName = Request.Form["Company"].ToString()
                });
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
            User user=_serviceWrapper._userService.FindUserById(CurrentUser.Id.Value);
            IndustryUser? industryUser = _serviceWrapper._industryUserService.FindIndustryUser(CurrentUser.Id.Value);
            return View("CurrentProfile",(user,industryUser));
        }
        public IActionResult ModifyProfile()
        {
            ViewBag.Role = GetCurrentRole();
            string? name = Request.Form["name"];
            string?email= Request.Form["email"];
            int? phone = Convert.ToInt32(Request.Form["phone"]);
            string? gender = Request.Form["gender"];
            string?description= Request.Form["description"];
            _serviceWrapper._userService.UpdateUser(CurrentUser.Id.Value,name,email,phone,gender,description);
            return View("Homepage");
        }

    }
}
