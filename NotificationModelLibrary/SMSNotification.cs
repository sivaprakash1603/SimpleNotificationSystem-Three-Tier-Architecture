using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationModelLibrary.Models
{
    public class SmsNotification : Notification
    {
        private const int MaxSmsLength = 160;

        public string ToPhoneNumber { get; set; } = string.Empty;

        public SmsNotification(string phoneNumber, string message) : base(message)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));
            }

            if (message.Length > MaxSmsLength)
            {
                throw new InvalidOperationException($"SMS message exceeds maximum length of {MaxSmsLength} characters. Current length: {message.Length}");
            }

            ToPhoneNumber = phoneNumber;
        }

        // Send SMS to phone number
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