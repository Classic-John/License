using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Datalayer.Interfaces;
using DataLayer.Models.Enums;
using Datalayer.Models.Wrapper;
using Relocation_and_booking_services;
using Datalayer.Models.Users;
using Datalayer.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Http;
using System.Linq;
namespace Relocation_and_booking_services.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        private readonly IHttpClientFactory _httpClientFactory;
        public static User CurrentUser { get; set; }
        private IndustryUser? IndustryUser { get; set; }
        private SchoolUser? SchoolUser { get; set; }
        private static bool FromLogin { get; set; } = true;
        public HomeController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService, IHttpClientFactory httpClientFactory)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
            _httpClientFactory = httpClientFactory;
        }

        public static int GetCurrentRole()
            => CurrentUser == null ? 0 : CurrentUser.Role.Equals("User") ? 1 : CurrentUser.Role.Equals("IndustryUser") ? 2 : 3;
        private IActionResult ViewSelection(int role)
        {
            switch (role)
            {
                case (int)Roles.User: return RedirectToAction("UserHome", "User");
                case (int)Roles.IndustryUser: return RedirectToAction("IndustryUserHome", "IndustryUser");
                case (int)Roles.SchoolUser: return RedirectToAction("SchoolUserView", "School");
                default: break;
            }
            return Homepage("Failed to load your user type view. Sending you back to the homepage.", Convert.ToBoolean(MessageType.Bad));
        }
        [Route("/")]
        [Route("Homepage")]
        [AllowAnonymous]
        public IActionResult Homepage(string? message = null, bool messageType = false)
            => View("Homepage", (message, messageType));

        [Route("Log in")]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            string name = Request.Form["Name"];
            string password = Request.Form["Password"];
            User? user = _serviceWrapper._userService.FindUserByName(name);
            if (user == null || user.Password.IsNullOrEmpty() || password.IsNullOrEmpty())
                return View("Failed", "Invalid user, please try again");
            bool result = BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password);
            if (!result)
                return View("Failed", "Wrong password, please try again");
            int userType = 0;
            foreach (Roles item in Enum.GetValues(typeof(Roles)))
            {
                if (Enum.GetName(item).Equals(user.Role))
                {
                    userType = (int)item;
                    break;
                }
            }
            CurrentUser = user;
            return ViewSelection(userType);
        }
        [Route("Sign out")]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync();
            CurrentUser = null;
            IndustryUser = null;
            SchoolUser = null;
            return Homepage("You have logged out successfully", Convert.ToBoolean(MessageType.Good));
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

        [Route("Create Account")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount()
        {
            int role = Convert.ToInt32(Request.Form["Option"]);
            if (_serviceWrapper._userService.FindUserByName(Request.Form["name"]) != null)
                return View("Failed", "Error, user already exists.");
            if (Request.Form["Email"].IsNullOrEmpty() || Request.Form["password"].IsNullOrEmpty())
                return View("Failed", "Error, no email or password added.");
            CurrentUser = await _serviceWrapper._userService.AddUser(new()
            {
                Name = Request.Form["Name"],
                Role = Enum.GetNames(typeof(Roles))[role],
                Gender = Convert.ToInt32(Request.Form["gender"]),
                Email = Request.Form["Email"].ToString(),
                Phone = ConvertNumber(Request.Form["Phone"].ToString().Replace(" ", "")),
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(Request.Form["password"], 14),
                ImageData = await ConvertImageToBytes(Request.Form.Files["photo"]),
                SelfDescription="",
                GoogleId = "",
            });
            if (role == (int)Roles.IndustryUser)
                await _serviceWrapper._industryUserService.AddIndustryUser(new()
                {
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id,
                    CompanyName = Request.Form["Company"].ToString(),
                    ServiceType = Convert.ToInt32(Request.Form["serviceType"])
                });
            if (role == (int)Roles.SchoolUser)
            {
                await _serviceWrapper._schoolService.AddSchoolUser(new()
                {
                    SchoolType = Convert.ToInt32(Request.Form["schoolType"]),
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id
                });
            }
            return ViewSelection(role);
        }
        [Route("CreateAccountWithGoogle")]
        public async Task CreateAccountWithGoogle()
        {
            int role = Convert.ToInt32(Request.Form["Option"]);
            CurrentUser = new()
            {
                Role = Enum.GetNames(typeof(Roles))[Convert.ToInt32(role)],
                Gender = Convert.ToInt32(Request.Form["gender"]),
                Phone = ConvertNumber(Request.Form["Phone"].ToString().Replace(" ", "")),
                SelfDescription="",
            };
            if (role == (int)Roles.IndustryUser)
                IndustryUser = new()
                {
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id,
                    CompanyName = Request.Form["Company"].ToString(),
                    ServiceType = Convert.ToInt32(Request.Form["serviceType"])
                };
            if (role == (int)Roles.SchoolUser)
            {
                SchoolUser = new()
                {
                    SchoolType = Convert.ToInt32(Request.Form["schoolType"]),
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id
                };
            }
            FromLogin = false;
            await GoogleLogin();
        }
        [Route("google-login")]
        [AllowAnonymous]
        public async Task GoogleLogin()
           => await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new() { RedirectUri = Url.Action("GoogleResponse") });


        private async Task<Stream> GetImageStreamAsync(string imageUrl)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
        [Route("google-signin")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value,
            }).ToList();
            string? googleId = claims.FirstOrDefault(claim => claim.Type.Equals("sub")).Value;
            User? user = _serviceWrapper._userService.FindUserByGoogleId(googleId);
            if (user == null)
            {
                if (FromLogin)
                    return View("Failed", "You don't have a google account added in your user profile.");
                string? email = claims.FirstOrDefault(claim => claim.Type.Equals("email")).Value;
                string? name = claims.FirstOrDefault(claim => claim.Type.Equals("name")).Value;
                string? picture = claims.FirstOrDefault(claim => claim.Type.Equals("picture")).Value;
                CurrentUser.Email = email;
                CurrentUser.Name = name;
                CurrentUser.GoogleId = googleId;
                CurrentUser.SelfDescription = "";
                CurrentUser.Password = "";
                Stream stream = await GetImageStreamAsync(picture);
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    CurrentUser.ImageData = memoryStream.ToArray();
                }
                await _serviceWrapper._userService.AddUser(CurrentUser);
                if (IndustryUser != null)
                    await _serviceWrapper._industryUserService.AddIndustryUser(IndustryUser);
                if (SchoolUser != null)
                    await _serviceWrapper._schoolService.AddSchoolUser(SchoolUser);
                IndustryUser = null;
                SchoolUser = null;
            }
            else if (!FromLogin)
                return View("Failed", "There's a user already using this google account on the platform, please use another google account or register normally.");
            else
                CurrentUser = user;
            FromLogin = true;
            return ViewSelection(GetCurrentRole());
        }
        [Route("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            int id = CurrentUser.Id;
            User? user = _serviceWrapper._userService.FindUserById(id);
            if (user is not User found)
                return View("Failed", "User doesn't exist.");
            if (found.Role.Equals("IndustryUser"))
                await _serviceWrapper._industryUserService.DeleteIndustryUser(id);
            else if (found.Role.Equals("SchoolUser"))
                await _serviceWrapper._schoolService.DeleteSchoolUser(id);
            await _serviceWrapper._userService.DeleteUser(id);
            return await Logout();
        }
        [Route("About Us")]
        [AllowAnonymous]
        public IActionResult AboutUs()
            => View("AboutUs");

        [Route("Booking Services")]
        public IActionResult AllBookings()
             => View("BookingServices", (_serviceWrapper._bookingService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [Route("Renting Services")]
        public IActionResult AllRentings()
            => View("RentingServices", (_serviceWrapper._rentingService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [Route("Furniture Transports Services")]
        public IActionResult AllFurnitureTransports()
            => View("AllFurnitureTransports", (_serviceWrapper._furnitureService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [Route("Jobs Services")]
        public IActionResult AllJobs()
            => View("AllJobs", (_serviceWrapper._jobService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [Route("All Transports")]
        public IActionResult AllTransports()
            => View("AllTransports", (_serviceWrapper._transportService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [Route("All Schools")]
        public IActionResult AllSchools()
            => View("AllSchools", (_serviceWrapper._schoolService.GetSchools(), _serviceWrapper._schoolService.GetSchoolUsers()));

        [Route("Profile")]
        public IActionResult Profile(string? message=null)
        {
            if (CurrentUser == null)
                return View("Failed", "You're not logged in yet. Only logged users can view their profiles.");
            User user = _serviceWrapper._userService.FindUserById(CurrentUser.Id);
            IndustryUser? industryUser = _serviceWrapper._industryUserService.FindIndustryUser(CurrentUser.Id);
            SchoolUser schoolUser = _serviceWrapper._schoolService.FindSchoolUser(CurrentUser.Id);
            return View("CurrentProfile", (user, industryUser, schoolUser,message));
        }
        public async Task<IActionResult> ModifyProfile()
        {
            string? name = Request.Form["name"];
            string? email = Request.Form["email"];
            long? phone = ConvertNumber(Request.Form["phone"].ToString().Replace(" ", ""));
            string? gender = Request.Form["gender"];
            string? description = Request.Form["description"];
            byte[]? newImage = Request.Form.Files.Count() <1 ? CurrentUser.ImageData:await ConvertImageToBytes(Request.Form.Files["newPhoto"]);
            await _serviceWrapper._userService.UpdateUser(CurrentUser.Id, name, email, phone, gender, description, newImage);
            if (CurrentUser.Role.Equals("IndustryUser"))
            {
                string? companyName = Request.Form["companyName"];
                int? serviceType = 0;
                foreach (int service in Enum.GetValues(typeof(ServiceTypes)))
                    if (nameof(service).Equals(Request.Form["serviceType"]))
                    {
                        serviceType = service;
                        break;
                    }
                if (serviceType == 0)
                    return Profile("Invalid service type. Please select the available services only.");
                IndustryUser user = _serviceWrapper._industryUserService.FindIndustryUser(CurrentUser.Id);
                user.CompanyName = companyName;
                user.ServiceType = serviceType;
                await _serviceWrapper._industryUserService.UpdateIndustryUser(user);
            }
            else if (CurrentUser.Role.Equals("SchoolUser"))
            {
                string? schoolType = Request.Form["schoolType"];
                int? acceptedType = 0;
                foreach(int school in Enum.GetValues(typeof(SchoolTypes)))
                    if(nameof(school).Equals(schoolType))
                    {
                        acceptedType = school;
                        break;
                    }
                if (acceptedType == 0)
                    return Profile("Invalid school type. Please select the available school types only.");
                SchoolUser user = _serviceWrapper._schoolService.FindSchoolUser(CurrentUser.Id);
                user.SchoolType = acceptedType;
                await _serviceWrapper._schoolService.UpdateSchoolUser(user);
            }
            return Homepage("Profile updated.",Convert.ToBoolean(MessageType.Good));
        }

        [Route("GetRole")]
        public int GetRole()
            => GetCurrentRole();
        private static long? ConvertNumber(string? number)
            => Convert.ToInt64(number.IsNullOrEmpty() ? 0 : number[0] == '0' ? number.Substring(1) : number);
    }
}
