using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Datalayer.Interfaces;

namespace Relocation_and_booking_services.Controllers
{

    public class IndustryUserController :Controller
    {
        private readonly IIndustryUserService _industryUserService;
        public IndustryUserController(IIndustryUserService industryUserService)
        {
            _industryUserService = industryUserService;
        }

        [Route("Industry User View")]
        public IActionResult IndustryUserHome() => View("IndustryUserView");
    }
}
