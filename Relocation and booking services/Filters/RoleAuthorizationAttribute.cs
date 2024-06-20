using Datalayer.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace Relocation_and_booking_services.Filters
{
    public class RoleAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly List<string?> _roles;
        public RoleAuthorizationAttribute(string? roles)
            => _roles = roles.Split(',').ToList();

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var sessionObject = context.HttpContext.Session.GetString("logged");
                if (sessionObject is null)
                    context.Result = new RedirectToActionResult("Homepage", "Home", "Log in first before accesing the platform");
                var loginDetails = JsonSerializer.Deserialize<User>(sessionObject);
                if (!_roles.Contains(loginDetails.Role))
                    context.Result = new RedirectToActionResult("Homepage", "Home", $"This feature isn't available for {loginDetails.Role}");
            }
            catch (Exception) { context.Result = new RedirectToActionResult("Homepage", "Home", "Log in first before accesing the platform");};
        }

    }
}
