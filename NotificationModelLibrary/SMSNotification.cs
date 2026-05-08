using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationModelLibrary.Models
{
    public class SmsNotification : Notification
    {
        public string ToPhoneNumber { get; set; } = string.Empty;

        public SmsNotification(string phoneNumber, string message) : base(message)
        {
            ToPhoneNumber = phoneNumber;
        }

        public override void Send(User user)
        {
            Console.WriteLine($"Sending SMS notification to {ToPhoneNumber} for {user.Name}...");
            Console.WriteLine($"Message: {Message}");
            Console.WriteLine($"Sent on: {SentDate}");
        }

        public override string ToString()
        {
            return $"SMS to: {ToPhoneNumber}, Message: {Message}, SentDate: {SentDate}";
        }
    }
}