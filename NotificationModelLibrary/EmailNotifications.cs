using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationModelLibrary.Models
{
    public class EmailNotification : Notification
    {
        public string ToEmail { get; set; } = string.Empty;

        public EmailNotification(string email, string message) : base(message)
        {
            ToEmail = email;
        }

        public override void Send(User user)
        {
            Console.WriteLine($"Sending email notification to {ToEmail} for {user.Name}...");
            Console.WriteLine($"Message: {Message}");
            Console.WriteLine($"Sent on: {SentDate}");
        }

        public override string ToString()
        {
            return $"Email to: {ToEmail}, Message: {Message}, SentDate: {SentDate}";
        }
    }
}