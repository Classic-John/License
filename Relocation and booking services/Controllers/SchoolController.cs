using Datalayer.Interfaces;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using static Relocation_and_booking_services.Controllers.HomeController;

namespace Relocation_and_booking_services.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public SchoolController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService,schoolService);
        }
        [Route("SchoolServices")]
        public IActionResult SchoolServices()
        {
            ViewBag.Role=GetCurrentRole();
            int id=Convert.ToInt32(Request.Form["userId"]);
            return View("PersonalSchoolList", _serviceWrapper._schoolService.GetSchoolServices(id));
        }
        [Route("UpdateSchoolOffer")]
        public IActionResult UpdateSchoolOffer()
        {
            return View();
        }
        [Route("AddSchoolOffer")]
        public IActionResult AddSchoolOffer()
        {
            return View();
        }
    }
}
