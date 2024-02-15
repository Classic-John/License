using Datalayer.Interfaces;
using Datalayer.Models.Email;
using Datalayer.Models.Users;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.XWPF.UserModel;

namespace Relocation_and_booking_services.Controllers
{
    //TO DO. Add dates (and maybe sort emails by one or more criterias)

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
                Email? email = new() { Body = currentEmail.Body, CreatorId = sender.Id, UserId = user.Id, Title = ($"[FORWARD] from {sender.Name}\n" + currentEmail.Title) };
                email.AddSenderAddress(currentEmail.GetSenderAddress());
                _serviceWrapper._userService.AddEmail(email);
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
            Email newMail = new() { Body = mail.Body, CreatorId = mail.UserId, UserId = mail.CreatorId, Title = ("[REPLY]\n" + mail.Title) };
            newMail.AddSenderAddress(mail.GetSenderAddress());
            _serviceWrapper._userService.AddEmail(newMail);
            return Emails();
        }
        [Route("CreateEmailView")]
        public IActionResult CreateEmailView()
            => View("CreateEmailView",_serviceWrapper._userService.GetUsers());

        [Route("SendEmail")]
        public IActionResult SendEmail()
        {
            string[] ids = Request.Form["ids"].ToString().Split(",");
            List<int> goodIds = ids.Select(id => Convert.ToInt32(id)).ToList();
            User? sender = _serviceWrapper._userService.FindUserById(HomeController.CurrentUser.Id.Value);
            string? title = ($"[SENT] from{sender.Name}\n with email: {sender.Email}\n\n "+Request.Form["title"]);
            string? body = ($"[SENT] from{sender.Name}\n with email: {sender.Email}\n\n " + Request.Form["body"]);
            foreach (int id in goodIds)
            {
                User? user = _serviceWrapper._userService.FindUserById(id);
                Email? senderEmail = _serviceWrapper._userService.AddEmail(new() { Title = title, Body = body, CreatorId = HomeController.CurrentUser.Id, UserId=user.Id });
                senderEmail.AddSenderAddress(sender.Email);
            }
            return Emails();
        }

    }
}
