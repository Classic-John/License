using Microsoft.AspNetCore.Mvc;
using Datalayer.Interfaces;
using DataLayer.Models.Enums;
using Datalayer.Models.Wrapper;
using Datalayer.Models.Users;
using Datalayer.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using System.Security.Claims;
using Relocation_and_booking_services.Filters;
using System.Text;
using Datalayer.Models.Email;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace Relocation_and_booking_services.Controllers
{
    [RequireHttps]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public static User CurrentUser { get; set; }
        private IndustryUser? IndustryUser { get; set; }
        private SchoolUser? SchoolUser { get; set; }
        private static bool FromLogin { get; set; } = true;
        private static bool FromSync { get; set; } = false;
        public HomeController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
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
        [HttpGet("/")]
        [HttpGet("Homepage")]
        public IActionResult Homepage(string? message = null, bool messageType = false)
            => View("Homepage", (message, messageType));
        private string? RandomString(int size, bool lowerCase)
        {
            StringBuilder partBuilder = new StringBuilder();
            Random random = new Random();
            char character;
            for (int i = 0; i < size; i++)
            {
                character = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                partBuilder.Append(character);
            }
            if (lowerCase)
                return partBuilder.ToString().ToLower();
            return partBuilder.ToString();
        }
        private int? RandomNumber(int? smallest, int? biggest)
            => new Random().Next(smallest.Value, biggest.Value);
        private string? GenerateRandomPassword()
        {
            StringBuilder passwordBuilder = new();
            passwordBuilder.Append(RandomString(5, true));
            passwordBuilder.Append(RandomNumber(10000, 99990));
            passwordBuilder.Append(RandomString(5, false));
            passwordBuilder.Append(RandomNumber(10000, 99999));
            return passwordBuilder.ToString();
        }

        [HttpPost("SMSPassword")]
        [IgnoreAntiforgeryToken]
        public IActionResult RecoverThroughSMS()
        {
            string? name = Request.Form["name"];
            string? phone = Request.Form["phone"];
            if (_serviceWrapper._userService.FindUserByName(name) is not User user)
                return Homepage("User doesn't exist", Convert.ToBoolean(MessageType.Bad));
            if (!user.Phone.ToString().Equals(phone))
                return Homepage("This user doesn't have this phone number", Convert.ToBoolean(MessageType.Bad));
            return SendSMS(user);
        }

        public IActionResult SendSMS(User? user)
        {
            var accountSid = _configuration["SMS:Id"];
            var authToken = _configuration["SMS:authToken"];
            try
            {
                TwilioClient.Init(accountSid, authToken);
            }
            catch (Exception) { return Homepage("The SMS service is not functional right now, please try again later.", Convert.ToBoolean(MessageType.Bad)); }
            var messageOptions = new CreateMessageOptions(
              new PhoneNumber($"+40{user.Phone}"));
            messageOptions.From = new PhoneNumber("+14237193664");
            string? newPassword = GenerateRandomPassword();
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, 14);
            _serviceWrapper._userService.UpdateUser(user);
            messageOptions.Body = $"Here is your new password: {newPassword}";

            try
            {
                var message = MessageResource.Create(messageOptions);
                Task.FromResult(message);
            }
            catch (Exception) { return Homepage($"Couldn't send SMS to {user.Phone}. It's either invalid or not from Romania."); }
            return Homepage("SMS send", Convert.ToBoolean(MessageType.Good));
        }

        [HttpPost("ForgotPassword")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ForgotPassword()
        {
            string? name = Request.Form["name"];
            string? emailAddress = Request.Form["email"];
            if (_serviceWrapper._userService.FindUserByName(name) is not User user)
                return Homepage("User doesn't exist", Convert.ToBoolean(MessageType.Bad));

            if (!user.Email.Equals(emailAddress))
                return Homepage("This user doens't have this email", Convert.ToBoolean(MessageType.Bad));
            string? newPassword = GenerateRandomPassword();

            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, 14);
            _serviceWrapper._userService.UpdateUser(user);
            Email? email = new Email() { Body = $"Here is your new password :{newPassword}\n\nDon't reply to this mail, i am not human.", Title = $"New password for {user.Name} account", Date = DateTime.Now };

            try { await EmailController.Execute(email, user, user); }
            catch (Exception) { return Homepage("Failed to send mail. Probably your email is wrong or is not part of a gmail account", Convert.ToBoolean(MessageType.Bad)); }

            return Homepage("Email sent to your personal gmail", Convert.ToBoolean(MessageType.Good));
        }
        [HttpPost("Log in")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> LogIn()
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
            await HiddenLogin(user);
            return ViewSelection(userType);
        }
        [HttpGet("Sign out")]
        [RoleAuthorization("User,IndustryUser,SchoolUser")]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync();
            CurrentUser = null;
            IndustryUser = null;
            SchoolUser = null;
            HttpContext.Session.Clear();
            return Homepage("You have logged out successfully", Convert.ToBoolean(MessageType.Good));
        }
        [HttpGet("KeepPicture")]
        public async Task<string> KeepPicture()
            => CurrentUser == null ? "/default.jpg" : CurrentUser.ImageData == null ? "/default.jpg" : await Task.FromResult($"data:image/jpg?v={DateTime.Now.Ticks};base64," + Convert.ToBase64String(CurrentUser.ImageData));
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

        [HttpPost("Create Account")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateAccount()
        {
            int role = Convert.ToInt32(Request.Form["Option"]);
            if (_serviceWrapper._userService.FindUserByName(Request.Form["name"]) != null)
                return View("Failed", "Error, user already exists.");
            if (Request.Form["Email"].IsNullOrEmpty() || Request.Form["password"].IsNullOrEmpty())
                return View("Failed", "Error, no email or password added.");
            long? phone = 0;
            try
            {
                phone = ConvertNumber(Request.Form["Phone"].ToString().Replace(" ", ""));
            }
            catch (Exception) { return View("Failed", "Error, invalid phone"); }

            CurrentUser = await _serviceWrapper._userService.AddUser(new()
            {
                Name = Request.Form["Name"],
                Role = Enum.GetNames(typeof(Roles))[role],
                Gender = Convert.ToInt32(Request.Form["gender"]),
                Email = Request.Form["Email"].ToString(),
                Phone = phone,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(Request.Form["password"], 14),
                ImageData = await ConvertImageToBytes(Request.Form.Files["photo"]),
                SelfDescription = "",
                GoogleId = "",
            });
            if (role == (int)Roles.IndustryUser)
            {
                if (string.IsNullOrEmpty(Request.Form["Company"]))
                    return View("Failed", "Company name is required");

                await _serviceWrapper._industryUserService.AddIndustryUser(new()
                {
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id,
                    CompanyName = Request.Form["Company"].ToString(),
                    ServiceType = Convert.ToInt32(Request.Form["serviceType"])
                });
            }
            if (role == (int)Roles.SchoolUser)
            {
                await _serviceWrapper._schoolService.AddSchoolUser(new()
                {
                    SchoolType = Convert.ToInt32(Request.Form["schoolType"]),
                    UserId = _serviceWrapper._userService.GetUsers().Last().Id
                });
            }
            await HiddenLogin(CurrentUser);
            return ViewSelection(role);
        }
        [HttpPost("CreateAccountWithGoogle")]
        [ValidateAntiForgeryToken]
        public async Task CreateAccountWithGoogle()
        {
            int role = Convert.ToInt32(Request.Form["Option"]);
            long? phone = 0;
            try
            {
                phone = ConvertNumber(Request.Form["Phone"].ToString().Replace(" ", ""));
            }
            catch (Exception) { View("Failed", "Error, invalid phone"); }
            CurrentUser = new()
            {
                Role = Enum.GetNames(typeof(Roles))[Convert.ToInt32(role)],
                Gender = Convert.ToInt32(Request.Form["gender"]),
                Phone = ConvertNumber(Request.Form["Phone"].ToString().Replace(" ", "")),
                SelfDescription = "",
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
        [HttpGet("syncVersion")]
        public async Task SyncGoogle()
        {
            FromSync = true;
            await GoogleLogin();
        }
        [HttpGet("google-login")]
        public async Task GoogleLogin()
           => await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new() { RedirectUri = Url.Action("GoogleResponse") });


        private async Task<Stream> GetImageStreamAsync(string imageUrl)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
        [HttpGet("DesyncGoogle")]
        public IActionResult DesyncGoogle()
        {
            if (string.IsNullOrEmpty(CurrentUser.GoogleId))
                return Homepage("Can't desync google account because user doesn't have his google account linked", Convert.ToBoolean(MessageType.Bad));
            CurrentUser.GoogleId = "";
            if (string.IsNullOrEmpty(CurrentUser.Password))
            {
                string? newPassword = GenerateRandomPassword();
                CurrentUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword);
                return RedirectToAction("MakePasswordForDesync", "Email", newPassword);
            }
            _serviceWrapper._userService.UpdateUser(CurrentUser);
            return Homepage();
        }
        [HttpGet("google-signin")]
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

            if (FromSync)
            {
                string? email = claims.FirstOrDefault(claim => claim.Type.Equals("email")).Value;
                string? name = claims.FirstOrDefault(claim => claim.Type.Equals("name")).Value;
                string? picture = claims.FirstOrDefault(claim => claim.Type.Equals("picture")).Value;
                CurrentUser.Email = email;
                CurrentUser.Name = name;
                CurrentUser.GoogleId = googleId;
                CurrentUser.SelfDescription = "";
                Stream stream = await GetImageStreamAsync(picture);
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    CurrentUser.ImageData = memoryStream.ToArray();
                }
                _serviceWrapper._userService.UpdateUser(CurrentUser);
                await HttpContext.SignOutAsync();
                await HiddenLogin(CurrentUser);
                FromSync = false;
                return Homepage($"Google account linked succesfully. Welcome {CurrentUser.Name}", Convert.ToBoolean(MessageType.Good));
            }
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
            await HiddenLogin(user);
            return ViewSelection(GetCurrentRole());
        }

        private async Task HiddenLogin(User user)
        {
            HttpContext.Session.SetString("logged", JsonSerializer.Serialize<User>(user));

            var theClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Name),
            };
            var claimIdentity = new ClaimsIdentity(theClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
        }
        [HttpPost("DeleteAccount")]
        [RoleAuthorization("User,IndustryUser,SchoolUser")]
        [ValidateAntiForgeryToken]
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
        [HttpGet("About Us")]
        public IActionResult AboutUs()
            => View("AboutUs");

        [HttpGet("Booking Services")]
        [RoleAuthorization("User")]
        public IActionResult AllBookings()
             => View("BookingServices", (_serviceWrapper._bookingService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [HttpGet("Renting Services")]
        [RoleAuthorization("User")]
        public IActionResult AllRentings()
            => View("RentingServices", (_serviceWrapper._rentingService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [HttpGet("Furniture Transports Services")]
        [RoleAuthorization("User")]
        public IActionResult AllFurnitureTransports()
            => View("AllFurnitureTransports", (_serviceWrapper._furnitureService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [HttpGet("Jobs Services")]
        [RoleAuthorization("User")]
        public IActionResult AllJobs()
            => View("AllJobs", (_serviceWrapper._jobService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [HttpGet("All Transports")]
        [RoleAuthorization("User")]
        public IActionResult AllTransports()
            => View("AllTransports", (_serviceWrapper._transportService.GetItems(), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [HttpGet("All Schools")]
        [RoleAuthorization("User")]
        public IActionResult AllSchools()
            => View("AllSchools", (_serviceWrapper._schoolService.GetSchools(), _serviceWrapper._schoolService.GetSchoolUsers()));

        [HttpGet("Profile")]
        [RoleAuthorization("User,IndustryUser,SchoolUser")]
        public IActionResult Profile(string? message = null)
        {
            if (CurrentUser == null)
                return View("Failed", "You're not logged in yet. Only logged users can view their profiles.");
            User user = _serviceWrapper._userService.FindUserById(CurrentUser.Id);
            IndustryUser? industryUser = _serviceWrapper._industryUserService.FindIndustryUser(CurrentUser.Id);
            SchoolUser schoolUser = _serviceWrapper._schoolService.FindSchoolUser(CurrentUser.Id);
            return View("CurrentProfile", (user, industryUser, schoolUser, message));
        }
        [HttpPost]
        [RoleAuthorization("User,IndustryUser,SchoolUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyProfile()
        {
            User? user1 = new();
            user1.Name = Request.Form["name"];
            user1.Email = Request.Form["email"];
            user1.Phone = ConvertNumber(Request.Form["phone"].ToString().Replace(" ", ""));
            user1.Gender = Request.Form["gender"].Equals("Male") ? 1 : Request.Form["gender"].Equals("Female") ? 2 : 3;
            user1.SelfDescription = Request.Form["description"];
            user1.ImageData = Request.Form.Files.Count() < 1 ? CurrentUser.ImageData : await ConvertImageToBytes(Request.Form.Files["newPhoto"]);
            user1.Id = CurrentUser.Id;
            user1.GoogleId = CurrentUser.GoogleId;
            user1.Role = CurrentUser.Role;
            user1.Password = CurrentUser.Password;
            _serviceWrapper._userService.UpdateUser(user1);
            CurrentUser.ImageData = user1.ImageData;
            if (CurrentUser.Role.Equals("IndustryUser"))
            {
                string? companyName = Request.Form["companyName"];
                int? serviceType = 0;
                int? count = 0;
                foreach (string service in Enum.GetNames(typeof(ServiceTypes)))
                {
                    count++;
                    if (service.Equals(Request.Form["serviceType"]))
                    {
                        serviceType = count;
                        break;
                    }
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
                int? count = 0;
                foreach (string school in Enum.GetNames(typeof(SchoolTypes)))
                {
                    count++;
                    if (school.Equals(schoolType))
                    {
                        acceptedType = count;
                        break;
                    }
                }
                if (acceptedType == 0)
                    return Profile("Invalid school type. Please select the available school types only.");
                SchoolUser user = _serviceWrapper._schoolService.FindSchoolUser(CurrentUser.Id);
                user.SchoolType = acceptedType;
                await _serviceWrapper._schoolService.UpdateSchoolUser(user);
            }
            return Homepage("Profile updated.", Convert.ToBoolean(MessageType.Good));
        }
        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword()
        {
            string? oldPassword = Request.Form["old"];
            string? newPassword = Request.Form["new"];
            string? retype = Request.Form["retype"];
            if (!newPassword.Equals(retype))
                return Homepage("Retype doesn't match the new password. Password change rejected", Convert.ToBoolean(MessageType.Bad));
            if (!BCrypt.Net.BCrypt.EnhancedVerify(oldPassword, CurrentUser.Password))
                return Homepage("Old password input doesn't match the actual old password. Password change rejected", Convert.ToBoolean(MessageType.Bad));
            CurrentUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword, 14);
            _serviceWrapper._userService.UpdateUser(CurrentUser);
            return Homepage("Password changed", Convert.ToBoolean(MessageType.Good));
        }

        [HttpGet("GetRole")]
        public int GetRole()
            => GetCurrentRole();
        private static long? ConvertNumber(string? number)
            => Convert.ToInt64(number.IsNullOrEmpty() ? 0 : number[0] == '0' ? number.Substring(1) : number);
    }
}
