using Core.Services;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Email;
using Datalayer.Models.Users;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Datalayer.Models.Enums;
using static Relocation_and_booking_services.Controllers.HomeController;
using System.Reflection;
using Datalayer.Models.SchoolItem;

namespace Relocation_and_booking_services.Controllers
{
    public class UserController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        private static List<int> ChosenServices = new();
        private static string CityOfOrigin { get; set; }
        private static string Destination { get; set; }
        public UserController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService,ISchoolService schoolService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
        }
        [Route("user home")]
        public IActionResult UserHome()
        {
            ViewBag.Role = GetCurrentRole();
            return View("UserView"); 
        }
        #region Iservice
        private bool ServiceAvailable(IService service)
            => service.GetCompanyNames().Contains(CityOfOrigin) || service.GetCompanyNames().Contains(Destination);
        private List<AbstractModel> FilterItems(IService service)
            => service.GetItems().Where(item => item.Location.Equals(Destination)).ToList();
        private AbstractModel? GetItem(IService service, int itemId)
            => service.GetItems().FirstOrDefault(item => item.Id == itemId);
        #endregion

        #region Utility Methods
        private IndustryUser? FindIndustryUser(int industryUserId)
            => _serviceWrapper._industryUserService.GetIndustryUsers().FirstOrDefault(user => user.UserId == industryUserId);
        #endregion

        [Route("relocation")]
        public IActionResult Relocation()
        {
            ViewBag.Role = GetCurrentRole();
            return View("Relocation", _serviceWrapper);
        }

        [Route("selected service")]
        public IActionResult ChosenService()
        {
            ViewBag.Role = GetCurrentRole();
            if (ChosenServices.Count == 0) 
                return View("Success");
            try
            {
                switch (ChosenServices[0])
                {
                    case 1:
                        if (ChosenServices.Count > 0)
                            ChosenServices.RemoveAt(0);
                        return View("BookingServices", FilterItems(_serviceWrapper._bookingService).Select(item => (Apartment)item).ToList());
                    case 2:
                        if (ChosenServices.Count > 0)
                            ChosenServices.RemoveAt(0);
                        return View("RentingServices", FilterItems(_serviceWrapper._rentingService).Select(item => (Vehicle)item).ToList());
                    case 3:
                        if (ChosenServices.Count > 0)
                            ChosenServices.RemoveAt(0);
                        return View("AllJobs", FilterItems(_serviceWrapper._jobService).Select(item => (Job)item).ToList());
                    case 4:
                        if (ChosenServices.Count > 0)
                            ChosenServices.RemoveAt(0);
                        return View("AllFurnitureTransports", FilterItems(_serviceWrapper._furnitureService).Select(item => (Furniture)item).ToList());
                    case 5:
                        if (ChosenServices.Count > 0)
                            ChosenServices.RemoveAt(0);
                        return View("AllTransports", FilterItems(_serviceWrapper._transportService).Select(item => (Transport)item).ToList());
                    case 6:
                        if (ChosenServices.Count > 0)
                            ChosenServices.RemoveAt(0);
                        return View("AllSchools",_serviceWrapper._schoolService.GetSchools().Where(item => item.Location.Equals(Destination)).ToList());

                }
            }
            catch { return View("Failed"); }
            return View("Success");
        }
        [Route("selected companies offers")]
        public IActionResult PersonalizedOffers()
        {
            ViewBag.Role = GetCurrentRole();
            CityOfOrigin = Request.Form["origin"].ToString();
            Destination = Request.Form["target"].ToString();

            foreach (string option in Request.Form.Keys)
            {
                if (!int.TryParse(Request.Form[option], out int outcome))
                {
                    continue;
                }
                ChosenServices.Add(outcome);
            }

            return ChosenService();
        }

        private bool SendEmail(int personId, int itemId, IService service, string serviceType)
        {
            IndustryUser? industryUser = FindIndustryUser(personId);

            if (industryUser == null)
                return false;

            AbstractModel? item = GetItem(service, itemId);

            if (item == null)
                return false;
            Email? industryEmail = EmailTexts.IndustryUserReceivedEmail(CurrentUser.Name, serviceType, personId, CurrentUser.Phone.Value,CurrentUser.Id.Value);

            if (industryEmail == null)
                return false;
            _serviceWrapper._userService.AddEmail(industryEmail);
            return true;
        }

        private bool SendSchoolEmail(int userId,int itemId, ISchoolService service) 
        {
            SchoolUser? user= service.FindSchoolUser(userId);
            User? actualUser=_serviceWrapper._userService.FindUserById(userId);
            if (user is not SchoolUser && actualUser is not null)
                return false;
            School? item = service.FindSchoolService(userId, itemId);
            if(item is not School) 
                return false;
            Email? schoolEmail = new() {
                Title = $"{actualUser.Name} applied to your school service", Body=$"{actualUser.Name} requests a place either for himself or his children at {item.Name}\n.Here is his phone: {actualUser.Password} and email: {actualUser.Email}"
                , Date=DateTime.Now, UserId=userId 
            };
            _serviceWrapper._userService.AddEmail(schoolEmail);
            return true;
        }
        [Route("Booking")]
        public IActionResult Booking()
        {
            ViewBag.Role = GetCurrentRole();
            int industryUserId = Convert.ToInt32(Request.Form["optionalId"].ToString());
            int apartmentId = Convert.ToInt32(Request.Form["objectId"]);
            bool sent = SendEmail(industryUserId, apartmentId, _serviceWrapper._bookingService, "Booking");
            return sent ? ChosenService() : View("Failed");
        }
        [Route("Renting")]
        public IActionResult Renting()
        {
            ViewBag.Role = GetCurrentRole();
            int industryUserId = Convert.ToInt32(Request.Form["optionalId"].ToString());
            int rentingId = Convert.ToInt32(Request.Form["objectId"]);
            bool sent = SendEmail(industryUserId, rentingId, _serviceWrapper._rentingService, "Renting");
            return sent ? ChosenService() : View("Failed");
        }
        [Route("ChosenJob")]
        public IActionResult FoundJob()
        {
            ViewBag.Role = GetCurrentRole();
            int industryUserId = Convert.ToInt32(Request.Form["optionalId"].ToString());
            int jobId = Convert.ToInt32(Request.Form["objectId"]);
            bool sent = SendEmail(industryUserId, jobId, _serviceWrapper._jobService, "Job Hunting");
            return sent ? ChosenService() : View("Failed");
        }
        [Route("Movers")]
        public IActionResult Movers()
        {
            ViewBag.Role = GetCurrentRole();
            int industryUserId = Convert.ToInt32(Request.Form["optionalId"].ToString());
            int furnitureId = Convert.ToInt32(Request.Form["objectId"]);
            bool sent = SendEmail(industryUserId, furnitureId, _serviceWrapper._furnitureService, "Furniture");
            return sent ? ChosenService() : View("Failed");
        }
        [Route("Travel")]
        public IActionResult Transport()
        {
            ViewBag.Role = GetCurrentRole();
            int industryUserId = Convert.ToInt32(Request.Form["optionalId"].ToString());
            int apartmentId = Convert.ToInt32(Request.Form["objectId"]);
            bool sent = SendEmail(industryUserId, apartmentId, _serviceWrapper._transportService, "Travel");
            return sent ? ChosenService() : View("Failed");
        }
        [Route("ChosenSchool")]
        public IActionResult ChosenSchool()
        {
            ViewBag.Role = GetCurrentRole();
            int userId = Convert.ToInt32(Request.Form["optionalId"].ToString());
            int objectId = Convert.ToInt32(Request.Form["objectId"]);
            bool sent = SendSchoolEmail(userId, objectId, _serviceWrapper._schoolService);
            return sent ? ChosenService() : View("Failed");
        }
    }
}
