namespace Relocation_and_booking_services.Start
{
    public class StartUp
    {
        public static bool DisallowsSameSiteNone(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return false;
            if (userAgent.Contains("CPU iPhone OS 12") ||
                userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}
