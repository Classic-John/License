using Bogus.DataSets;
using Datalayer.Interfaces;
using Datalayer.Models.Email;
using Datalayer.Models.Users;
using Datalayer.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using NPOI.XWPF.UserModel;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using System.Net.Mail;
using static Relocation_and_booking_services.Controllers.HomeController;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Relocation_and_booking_services.Filters;
using Relocation_and_booking_services.Pages.Home;


namespace Relocation_and_booking_services.Controllers
{

    [RequireHttps]
    [Route("Email")]
    [RoleAuthorization("User,IndustryUser,SchoolUser")]
    public class EmailController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;
        private readonly IConfiguration _configuration;
        private static string? _emailKey;
        public EmailController(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService,
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService, IConfiguration configuration)
        {
            _serviceWrapper = new(bookingService, furnitureService, jobService, rentingService, transportService, userService, industryUserService, schoolService);
            _configuration = configuration;
            _emailKey = _configuration["EmailKey"];
        }
        [HttpGet("NewEmailsNumber")]
        public async Task<int> GetNewEmails()
            => CurrentUser == null ? 0 : _serviceWrapper._userService.GetNumberOfNewEmails(CurrentUser.Id).Count();
        [HttpGet("Emails")]
        public IActionResult Emails(string? message = null)
            => View("EmailList", (_serviceWrapper._userService.GetShorterEmails(CurrentUser.Id), message));


        [HttpPost("ViewEmail")]
        [ValidateAntiForgeryToken]
        public IActionResult ViewEmail()
        {
            int emailId = Convert.ToInt32(Request.Form["objectId"].ToString());
            Email? selectedEmail = _serviceWrapper._userService.FindEmail(emailId);
            selectedEmail.Opened = true;
            _serviceWrapper._userService.UpdateAsOpened(selectedEmail);
            byte[]? creatorImage = null;
            User? user = _serviceWrapper._userService.FindUserById(selectedEmail.CreatorId.Value);
            if (user != null)
                creatorImage = user.ImageData;
            return View("Email", (selectedEmail, _serviceWrapper._userService.GetUsers(), creatorImage));
        }
        [HttpPost("DeleteEmail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmail()
        {
            int? emailId = Request.Form.ContainsKey("objectId") ? Convert.ToInt32(Request.Form["objectId"]) : Convert.ToInt32(Request.Form["mailId"]);
            string? message = "Mail has been deleted";
            try { await _serviceWrapper._userService.DeleteEmail(emailId.Value); }
            catch (Exception) { message = "#Email doesn't exist"; }
            return Emails(message);
        }
        [HttpPost("Forward")]
        [ValidateAntiForgeryToken]
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
                Email? email = new() { Body = currentEmail.Body, CreatorId = sender.Id, UserId = user.Id, Title = ($"[FORWARD] from {sender.Name}\n" + currentEmail.Title), Date = DateTime.Now, Opened = false };
                email.AddSenderAddress(currentEmail.GetSenderAddress());
                await _serviceWrapper._userService.AddEmail(email);
            }
            return Emails($"Mail has been forwarded to {goodIds.Count()} users.");
        }
        [HttpPost("Reply")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToMail()
        {
            int? mailId = Convert.ToInt32(Request.Form["mailId"]);
            mailId = mailId == 0 ? 1 : mailId;
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);

            if (_serviceWrapper._userService.FindUserById(currentEmail.UserId.Value) is not User user || currentEmail.CreatorId < 0)
                return Emails("#User doesn't exist. Please try again.");

            string? newBody = $"\n\n[Reply] from {user.Name}\n with email address:{user.Email}:\n\n {Request.Form["replyBlockData"]}";
            Email? mail = await _serviceWrapper._userService.ModifyEmail(mailId.Value, newBody);
            Email newMail = new() { Body = mail.Body, CreatorId = mail.UserId, UserId = mail.CreatorId, Title = ("[REPLY]\n" + mail.Title), Date = DateTime.Now, Opened = false };
            newMail.AddSenderAddress(mail.GetSenderAddress());
            await _serviceWrapper._userService.AddEmail(newMail);
            string? name = _serviceWrapper._userService.FindUserById(currentEmail.CreatorId.Value).Name;
            return Emails($"Reply sent to {name}.");
        }
        [HttpPost("SendToEmailSenderGmail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToEmailSenderGmail()
        {
            int? mailId = Convert.ToInt32(Request.Form["mailId"]);
            mailId = mailId == 0 ? 1 : mailId;
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);

            if (_serviceWrapper._userService.FindUserById(currentEmail.CreatorId.Value) is not User user || currentEmail.CreatorId < 0)
                return Emails("#User doesn't exist. Please try again.");
            string? newBody = $"\n\n[Reply With GOOGLE] from {user.Name}\n with email address:{user.Email}:\n\n {Request.Form["replyBlockData"]}";
            Email? mail = await _serviceWrapper._userService.ModifyEmail(mailId.Value, newBody);
            Email newMail = new() { Body = mail.Body, CreatorId = mail.UserId, UserId = mail.CreatorId, Title = ("[REPLY With GOOGLE]\n" + mail.Title), Date = DateTime.Now, Opened = false };
            newMail.AddSenderAddress(mail.GetSenderAddress());
            string? message = (await Execute(newMail, user, user)) ? $"Email delivered to {user.Email}" : $"#Failed to deliver email to {user.Email}";
            return Emails(message);
        }
        [HttpGet("CreateEmailView")]
        public IActionResult CreateEmailView()
            => View("CreateEmailView", _serviceWrapper._userService.GetUsers());
        [HttpPost("SendToGmail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToGmail()
        {
            int? mailId = Convert.ToInt32(Request.Form["mailId"]);
            mailId = mailId == 0 ? 1 : mailId;
            Email? currentEmail = _serviceWrapper._userService.FindEmail(mailId.Value);
            string? message = (await Execute(currentEmail, CurrentUser, CurrentUser)) ? $"Email delivered to {CurrentUser.Email}" : $"#Failed to deliver email to {CurrentUser.Email}";
            return Emails(message);
        }
        public static async Task<bool> Execute(Email? email, User? fromUser, User? toUser)
        {
            if (fromUser is null || toUser is null)
                return false;

            var apiKey = _emailKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromUser.Email, fromUser.Name);
            var subject = $"[FORWARDED FROM Relocation and Booking services]\n {email.Title}";
            var to = new EmailAddress(toUser.Email, toUser.Name);
            var plainTextContent = email.Body;
            email.Body = email.Body.Replace("\n", "<br>");
            var htmlContent = $"<strong>{email.Body}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
        [HttpGet("MakePasswordForDesync")]
        public IActionResult MakePasswordForDesync(string? password)
        {
            Email? mail = new() { CreatorId = -1, Date = DateTime.Now, Opened = false, Title = $"New account password for your account", UserId = CurrentUser.Id };
            mail.Body = $"Your new password is {password}";
            Execute(mail, CurrentUser, CurrentUser).Wait();
            return RedirectToAction("Homepage", "Home");
        }
        [HttpPost("SendEmail")]
        [ValidateAntiForgeryToken]
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
                Email? senderEmail = await _serviceWrapper._userService.AddEmail(new() { Title = title, Body = body, CreatorId = CurrentUser.Id, UserId = user.Id, Date = DateTime.Now, Opened = false });
                senderEmail?.AddSenderAddress(sender.Email);
            }
            return Emails($"Email sent to {goodIds.Count()} users");
        }
    }
}
