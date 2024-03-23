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
    [ValidateAntiForgeryToken]
    public class IndustryUserController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public IndustryUserController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
        }

        [Route("Industry User View")]
        public IActionResult IndustryUserHome() 
            => View("IndustryUserView");
        [Route("Service List")]
        public IActionResult ServiceList() 
            => View("PersonalServiceList", (_serviceWrapper._industryUserService.GetServiceList(CurrentUser.Id), _serviceWrapper._industryUserService.GetIndustryUsers()));

        [Route("Delete")]
        public async Task<IActionResult> Delete()
        {
            int? creatorId = Convert.ToInt32(Request.Form["creatorOfItemId"].ToString());
            int? itemId = Convert.ToInt32(Request.Form["deleteItemId"].ToString());
            IndustryUser? user = _serviceWrapper._industryUserService.FindIndustryUser(creatorId.Value);
            switch (user.ServiceType)
            {
                case (int)ServiceTypes.Booking:
                    await _serviceWrapper._bookingService.RemoveApartment((Apartment)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Renting:
                    await _serviceWrapper._rentingService.RemoveVehicle((Vehicle)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Job:
                    await _serviceWrapper._jobService.RemoveJob((Job)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Furniture:
                    await _serviceWrapper._furnitureService.RemoveFurnitureTransport((Furniture)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
                case (int)ServiceTypes.Transport:
                    await _serviceWrapper._transportService.RemoveTransport((Transport)_serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value));
                    break;
            }
            return ServiceList();
        }
        [Route("UpdateOffer")]
        public async Task<IActionResult> UpdateOffer()
        {
            int? itemId = Convert.ToInt32(Request.Form["chosenItemId"].ToString());
            int? creatorId = Convert.ToInt32(Request.Form["currentUserId"].ToString());
            AbstractModel? item = _serviceWrapper._industryUserService.GetItem(creatorId.Value, itemId.Value);
            item.Description = Request.Form["offerDescription"];
            item.Price = Convert.ToInt32(Request.Form["offerPrice"]);
            item.Link = Request.Form["offerLink"];
            item.Name = Request.Form["offerTitle"];
            item.Location = Request.Form["offerLocation"];
            item.Image = await ConvertImageToBytes(Request.Form.Files["newPhoto"]);
            item.Date = DateTime.Now;
            switch (_serviceWrapper._industryUserService.FindIndustryUser(item.CreatorId).ServiceType)
            {
                case (int)ServiceTypes.Booking:
                    await _serviceWrapper._bookingService.UpdateApartment((Apartment)item);
                    break;
                case (int)ServiceTypes.Renting:
                    await _serviceWrapper._rentingService.UpdateVehicle((Vehicle)item);
                    break;
                case (int)ServiceTypes.Job:
                    await _serviceWrapper._jobService.UpdateJob((Job)item);
                    break;
                case (int)ServiceTypes.Furniture:
                    await _serviceWrapper._furnitureService.UpdateFurniture((Furniture)item);
                    break;
                case (int)ServiceTypes.Transport:
                    await _serviceWrapper._transportService.UpdateTransport((Transport)item);
                    break;
            }
            return ServiceList();

        }
        [Route("AddOffer")]
        public async Task<IActionResult> AddOffer()
        {
            int? creatorId = CurrentUser.Id;
            string? title = Request.Form["offerTitle"];
            string? description = Request.Form["offerDescription"];
            int? price = Convert.ToInt32(Request.Form["offerPrice"]);
            string? link = Request.Form["offerLink"];
            string? location = Request.Form["offerLocation"];
            byte[]? newImage = ConvertImageToBytes(Request.Form.Files["newPhoto"]).Result;
            IndustryUser? user = _serviceWrapper._industryUserService.FindIndustryUser(creatorId.Value);
            switch (user.ServiceType)
            {
                case (int)ServiceTypes.Booking:
                    await _serviceWrapper._bookingService.AddApartment(new() { CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date = DateTime.Now });
                    break;
                case (int)ServiceTypes.Renting:
                    await _serviceWrapper._rentingService.AddVehicle(new() { CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date = DateTime.Now });
                    break;
                case (int)ServiceTypes.Job:
                    await _serviceWrapper._jobService.AddJob(new() { CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date = DateTime.Now });
                    break;
                case (int)ServiceTypes.Furniture:
                    await _serviceWrapper._furnitureService.AddFurnitureTransport(new() { CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date = DateTime.Now });
                    break;
                case (int)ServiceTypes.Transport:
                    await _serviceWrapper._transportService.AddTransport(new() { CreatorId = user.UserId.Value, Description = description, Link = link, Name = title, Price = price, Location = location, Image = newImage, Date = DateTime.Now });
                    break;
            }
            return ServiceList();
        }
    }
}
