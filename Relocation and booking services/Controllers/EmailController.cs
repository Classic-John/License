using Bogus.DataSets;
using Datalayer.Interfaces;
using Datalayer.Models.Email;
using Datalayer.Models.Users;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.XWPF.UserModel;
using static Relocation_and_booking_services.Controllers.HomeController;

namespace Relocation_and_booking_services.Controllers
{
    //TO DO. Sort emails by one or more criterias)

    [Route("Email")]
    public class EmailController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        public EmailController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
        }
        [Route("Emails")]
        public IActionResult Emails()
            => View("EmailList", _serviceWrapper._userService.GetShorterEmails(CurrentUser.Id));


        [Route("ViewEmail")]
        public IActionResult ViewEmail()
        {
            int emailId = Convert.ToInt32(Request.Form["objectId"].ToString());
            Email? selectedEmail = _serviceWrapper._userService.FindEmail(emailId);
            byte[]? creatorImage = null;
            try { creatorImage = _serviceWrapper._userService.FindUserById(selectedEmail.CreatorId.Value).ImageData; }
            catch (Exception) { creatorImage = null; }
            return View("Email", (selectedEmail, _serviceWrapper._userService.GetUsers(), creatorImage));
        }
        [Route("DeleteEmail")]
        public async Task<IActionResult> DeleteEmail()
        {
            int? emailId = Request.Form.ContainsKey("objectId") ? Convert.ToInt32(Request.Form["objectId"]) : Convert.ToInt32(Request.Form["mailId"]);
            await _serviceWrapper._userService.DeleteEmail(emailId.Value);
            return Emails();
        }
        [Route("Forward")]
        public async Task<IActionResult> ForwardMail()
        {
            string[] ids = Request.Form["ids"].ToString().Split(",");
            List<int> goodIds = ids.Select(id => Convert.ToInt32(id)).ToList();
            int? mailId = Convert.ToInt32(Request.Form["mailId"]);
            mailId = mailId == 0 ? 1 : mailId;
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);
            User? sender = _serviceWrapper._userService.FindUserById(currentEmail.UserId.Value);
            foreach (int goodId in goodIds)
            {
                User? user = _serviceWrapper._userService.FindUserById(goodId);
                Email? email = new() { Body = currentEmail.Body, CreatorId = sender.Id, UserId = user.Id, Title = ($"[FORWARD] from {sender.Name}\n" + currentEmail.Title), Date = DateTime.Now };
                email.AddSenderAddress(currentEmail.GetSenderAddress());
                await _serviceWrapper._userService.AddEmail(email);
            }
            return Emails();
        }
        [Route("Reply")]
        public async Task<IActionResult> ReplyToMail()
        {
            int? mailId = Convert.ToInt32(Request.Form["mailId"]);
            mailId = mailId == 0 ? 1 : mailId;
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);
            if (_serviceWrapper._userService.FindUserById(currentEmail.UserId.Value) is not User user)
            {
                return Emails();
            }
            string? newBody = $"\n\n[Reply] from {user.Name}\n with email address:{user.Email}:\n\n {Request.Form["replyBlockData"]}";
            Email? mail = await _serviceWrapper._userService.ModifyEmail(mailId.Value, newBody);
            Email newMail = new() { Body = mail.Body, CreatorId = mail.UserId, UserId = mail.CreatorId, Title = ("[REPLY]\n" + mail.Title), Date = DateTime.Now };
            newMail.AddSenderAddress(mail.GetSenderAddress());
            await _serviceWrapper._userService.AddEmail(newMail);
            return Emails();
        }
        [Route("CreateEmailView")]
        public IActionResult CreateEmailView() 
            => View("CreateEmailView", _serviceWrapper._userService.GetUsers());

        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail()
        {
            string[] ids = Request.Form["ids"].ToString().Split(",");
            List<int> goodIds = ids.Select(id => Convert.ToInt32(id)).ToList();
            User? sender = _serviceWrapper._userService.FindUserById(CurrentUser.Id);
            string? title = ($"[SENT] from{sender.Name}\n with email: {sender.Email}\n\n " + Request.Form["title"]);
            string? body = ($"[SENT] from{sender.Name}\n with email: {sender.Email}\n\n " + Request.Form["body"]);
            foreach (int id in goodIds)
            {
                User? user = _serviceWrapper._userService.FindUserById(id);
                Email? senderEmail = await _serviceWrapper._userService.AddEmail(new() { Title = title, Body = body, CreatorId = CurrentUser.Id, UserId = user.Id, Date = DateTime.Now });
                senderEmail?.AddSenderAddress(sender.Email);
            }
            return Emails();
        }

    }
}
