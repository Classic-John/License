using Datalayer.Interfaces;
using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Relocation_and_booking_services.Filters;
using static Relocation_and_booking_services.Controllers.HomeController;

namespace Relocation_and_booking_services.Controllers
{
    [RequireHttps]
    [Route("School")]
    [RoleAuthorization("SchoolUser")]
    public class SchoolController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public SchoolController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
        }
        [Route("School User Logged")]
        public IActionResult SchoolUserView()
            => View("SchoolUserView", $"Welcome {CurrentUser.Name}, you have logged as an school user.");
        
        [Route("SchoolServices")]
        public IActionResult SchoolServices(string? message = null) 
            => View("PersonalSchoolList", (_serviceWrapper._schoolService.GetSchoolServices(CurrentUser.Id),message));

        [Route("UpdateSchoolOffer")]
        public async Task<IActionResult> UpdateSchoolOffer()
        {
            string? schoolName = Request.Form["offerTitle"];
            string? schoolDescription = Request.Form["offerDescription"];
            string? schoolLink = Request.Form["offerLink"];
            string? schoolLocation = Request.Form["offerLocation"];
            byte[]? newImage = ConvertImageToBytes(Request.Form.Files["newPhoto"]).Result;
            School? item = _serviceWrapper._schoolService.FindSchoolService(CurrentUser.Id, Convert.ToInt32(Request.Form["chosenItemId"]));
            item.Name = schoolName;
            item.Description = schoolDescription;
            item.Link = schoolLink;
            item.Location = schoolLocation;
            item.Image = newImage;
            await _serviceWrapper._schoolService.UpdateSchool(item);
            return SchoolServices("School offer updated.");
        }
        [Route("AddSchoolOffer")]
        public async Task<IActionResult> AddSchoolOffer()
        {
            string? schoolName = Request.Form["offerTitle"];
            string? schoolDescription = Request.Form["offerDescription"];
            string? schoolLink = Request.Form["offerLink"];
            string? schoolLocation = Request.Form["offerLocation"];
            byte[]? newImage = ConvertImageToBytes(Request.Form.Files["newPhoto"]).Result;
            await _serviceWrapper._schoolService.AddSchoolItem(new() { Name = schoolName, Description = schoolDescription, Link = schoolLink, Location = schoolLocation, Image = newImage, Date = DateTime.Now, CreatorId = CurrentUser.Id });
            return SchoolServices($"New school offer added to your list: {schoolName}.");
        }
        [Route("DeleteSchoolOffer")]
        public async Task<IActionResult> DeleteSchoolOffer()
        {
            School? item = _serviceWrapper._schoolService.FindSchoolService(CurrentUser.Id, Convert.ToInt32(Request.Form["deleteItemId"]));
            await _serviceWrapper._schoolService.RemoveSchoolService(item);
            return SchoolServices("School offer deleted.");
        }
    }
}
