using Datalayer.Interfaces;
using Datalayer.Models.Email;
using Datalayer.Models.Users;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Spreadsheet;

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
            int emailId = Convert.ToInt32(Request.Form["mailId"].ToString());
            Email? selectedEmail = _serviceWrapper._userService.FindEmail(emailId);
            return View("Email", selectedEmail);
        }
        [Route("DeleteEmail")]
        public IActionResult DeleteEmail()
        {
            int emailId = Convert.ToInt32(Request.Form["mailId"].ToString());
            _serviceWrapper._userService.DeleteEmail(emailId);
            return RedirectToAction("Emails", "User");
        }
        [Route("Forward")]
        public IActionResult ForwardMail()
        {
            return View();
        }
        [Route("Reply")]
        public IActionResult ReplyToMail()
        {
            int? mailId = Convert.ToInt32(Request.Form["viewedEmailId"]);
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);
            User? user = _serviceWrapper._userService.FindUserById(currentEmail.UserId.Value);
            string? newBody = $"\n\nReply from {user.Name}\n with email address:{user.Email}:\n\n {Request.Form["replyBlockData"]}";
            Email? mail = _serviceWrapper._userService.ModifyEmail(mailId.Value, newBody);
            _serviceWrapper._userService.AddEmail(new() { Body = mail.Body, CreatorId = mail.UserId, EmailAddress = mail.EmailAddress, UserId = mail.CreatorId, Id = mail.Id, Title = mail.Title });
            return RedirectToAction("Emails", "User");
        }

    }
}
