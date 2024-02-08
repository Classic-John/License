using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Email
{
    public static class EmailTexts
    {
        static (string Title, string Body) SuccessEmail = new() { Title = "Message sent", Body = "Your email has been sent to the desired person" };
        static (string Title, string Body) FailEmail = new() { Title = "Error", Body = "Your email couldn't be sent" };

        public static Email? IndustryUserReceivedEmail(string? personName, string? applicationType, int userId, int phoneNumber,int senderId)
            => (personName == null || applicationType == null) ? null :
                new() { Id = 3, UserId = userId, Title = $"{personName} has applied for your {applicationType} service", Body = $"Here is his phone: {phoneNumber}", CreatorId=senderId };
    }
}
