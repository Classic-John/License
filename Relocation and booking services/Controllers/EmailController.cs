using Datalayer.Interfaces;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace Relocation_and_booking_services.Controllers
{
    [Route("Email")]
    public class EmailController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public EmailController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService);
        }
        [Route("ViewEmail")]
        public IActionResult ViewEmail()
        {
            return View();
        }
       
    }
}
