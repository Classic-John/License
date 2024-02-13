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
        [Route("Emails")]
        public IActionResult Emails()
          => View("EmailList", _serviceWrapper._userService.GetShorterEmails(HomeController.CurrentUser.Id.Value));

        [Route("ViewEmail")]
        public IActionResult ViewEmail()
        {
            int emailId = Convert.ToInt32(Request.Form["mailId"].ToString());
            Email? selectedEmail = _serviceWrapper._userService.FindEmail(emailId);
            return View("Email", (selectedEmail,_serviceWrapper._userService.GetUsers()));
        }
        [Route("DeleteEmail")]
        public IActionResult DeleteEmail()
        {
            int emailId = Convert.ToInt32(Request.Form["mailId"].ToString());
            _serviceWrapper._userService.DeleteEmail(emailId);
            return Emails();
        }
        [Route("Forward")]
        public IActionResult ForwardMail()
        {
            string[] ids = Request.Form["ids"].ToString().Split(",");
            List<int> goodIds=ids.Select(id => Convert.ToInt32(id)).ToList();
            int? mailId = Convert.ToInt32(Request.Form["mailId"]);
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);
            User? sender = _serviceWrapper._userService.FindUserById(currentEmail.UserId.Value);
            foreach (int goodId in goodIds)
            {
                User? user = _serviceWrapper._userService.FindUserById(goodId);
                _serviceWrapper._userService.AddEmail(new() { Body = currentEmail.Body, CreatorId = sender.Id, EmailAddress = currentEmail.EmailAddress, UserId = user.Id, Title = ($"[FORWARD] from {sender.Name}\n" + currentEmail.Title) });
            }
            return Emails();
        }
        [Route("Reply")]
        public IActionResult ReplyToMail()
        {
            int? mailId = Convert.ToInt32(Request.Form["mailId"]);
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);
            User? user = _serviceWrapper._userService.FindUserById(currentEmail.UserId.Value);
            string? newBody = $"\n\n[Reply] from {user.Name}\n with email address:{user.Email}:\n\n {Request.Form["replyBlockData"]}";
            Email? mail = _serviceWrapper._userService.ModifyEmail(mailId.Value, newBody);
            _serviceWrapper._userService.AddEmail(new() { Body = mail.Body, CreatorId = mail.UserId, EmailAddress = mail.EmailAddress, UserId = mail.CreatorId, Title = ("[REPLY]\n"+mail.Title) });
            return Emails();
        }
        [Route("CreateEmailView")]
        public IActionResult CreateEmailView()
            => View("CreateEmailView");

    }
}
