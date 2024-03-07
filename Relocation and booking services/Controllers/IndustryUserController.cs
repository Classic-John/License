using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Datalayer.Interfaces;
using Datalayer.Models.Wrapper;
using Datalayer.Models.Enums;
using NPOI.XWPF.UserModel;
using Datalayer.Models.Users;
using Datalayer.Models;
using static Relocation_and_booking_services.Controllers.HomeController;
namespace Relocation_and_booking_services.Controllers
{
    [Route("IndustryUser")]
    public class IndustryUserController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public IndustryUserController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService,ISchoolService schoolService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
        }

        [Route("Industry User View")]
        public IActionResult IndustryUserHome()
        {
            ViewBag.Role = GetCurrentRole();
            return View("IndustryUserView");
        }
        [Route("Service List")]
        public IActionResult ServiceList()
        {
            ViewBag.Role = GetCurrentRole();
            return View("PersonalServiceList", _serviceWrapper._industryUserService.GetServiceList(CurrentUser.Id.Value));
        }

        [Route("Delete")]
        public IActionResult Delete()
        {
            ViewBag.Role = GetCurrentRole();
            int? creatorId = Convert.ToInt32(Request.Form["creatorOfItemId"].ToString());
            int? itemId = Convert.ToInt32(Request.Form["deleteItemId"].ToString());
            IndustryUser? user = _serviceWrapper._industryUserService.FindIndustryUser(creatorId.Value);
            switch (user.ServiceType)
            {
                case (int)ServiceTypes.Booking:
                    _serviceWrapper._bookingService.RemoveApartment((Apartment)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Renting:
                    _serviceWrapper._rentingService.RemoveVehicle((Vehicle)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Job:
                    _serviceWrapper._jobService.RemoveJob((Job)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Furniture:
                    _serviceWrapper._furnitureService.RemoveFurnitureTransport((Furniture)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Transport:
                    _serviceWrapper._transportService.RemoveTransport((Transport)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
            }
            return ServiceList();
        }
        [Route("UpdateOffer")]
        public IActionResult UpdateOffer()
        {
            ViewBag.Role = GetCurrentRole();
            int? itemId = Convert.ToInt32(Request.Form["chosenItemId"].ToString());
            int? creatorId = Convert.ToInt32(Request.Form["currentUserId"].ToString());
            AbstractModel? item = _serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value);
            item.Description = Request.Form["offerDescription"];
            item.Price = Convert.ToInt32(Request.Form["offerPrice"]);
            item.Link = Request.Form["offerLink"];
            item.Name = Request.Form["offerTitle"];
            item.Location = Request.Form["offerLocation"];
            item.Image = ConvertImageToBytes(Request.Form.Files["newPhoto"]).Result;
            item.Date=DateTime.Now;
            return ServiceList();

        }
        [Route("AddOffer")]
        public IActionResult AddOffer()
        {
            ViewBag.Role = GetCurrentRole();
            int? creatorId =CurrentUser.Id;
            string? title = Request.Form["offerTitle"];
            string? description = Request.Form["offerDescription"];
            int? price = Convert.ToInt32(Request.Form["offerPrice"]);
            string? link = Request.Form["offerLink"];
            string? location = Request.Form["offerLocation"];
            byte[]? newImage= ConvertImageToBytes(Request.Form.Files["newPhoto"]).Result;
            IndustryUser? user = _serviceWrapper._industryUserService.FindIndustryUser(creatorId.Value);
            switch (user.ServiceType)
            {
                case (int)ServiceTypes.Booking:
                    _serviceWrapper._bookingService.AddApartment(new() {CompanyName=user.CompanyName, CreatorId=user.UserId.Value, Description=description, Link=link, Name=title, Price=price, Location=location, Image=newImage, Date=DateTime.Now });
                    break;
                case (int)ServiceTypes.Renting:
                    _serviceWrapper._rentingService.AddVehicle(new() {CompanyName = user.CompanyName, CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date = DateTime.Now });
                    break;
                case (int)ServiceTypes.Job:
                    _serviceWrapper._jobService.AddJob(new() { CompanyName = user.CompanyName, CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date=DateTime.Now });
                    break;
                case (int)ServiceTypes.Furniture:
                    _serviceWrapper._furnitureService.AddFurnitureTransport(new() { CompanyName = user.CompanyName, CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date=DateTime.Now });
                    break;
                case (int)ServiceTypes.Transport:
                    _serviceWrapper._transportService.AddTransport(new() { CompanyName = user.CompanyName, CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date=DateTime.Now });
                    break;
            }
            return ServiceList();
        }
    }
}
