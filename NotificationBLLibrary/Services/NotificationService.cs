using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationBLLibrary.Interfaces;
using NotificationModelLibrary.Exceptions;
using NotificationModelLibrary.Models;

namespace NotificationBLLibrary.Services
{
    public class NotificationService : ISendNotification
    {
        public Notification SendEmailNotification(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            Console.WriteLine($"Sending email notification to {email}...");
            Console.WriteLine("Enter the message of the email:");
            string message = Console.ReadLine() ?? string.Empty;

            return new EmailNotification(email, message);
        }

        public Notification SendSMSNotification(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));
            }

            Console.WriteLine($"Sending SMS notification to {phoneNumber}...");
            Console.WriteLine("Enter the message of the SMS:");
            string message = Console.ReadLine() ?? string.Empty;

            return new SmsNotification(phoneNumber, message);
        }

        public void SendNotificationToUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            int option;
            Console.WriteLine("Choose notification type:");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. SMS");
            while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
            {
                Console.WriteLine("Invalid input. Please enter 1 for Email or 2 for SMS.");
            }

            Notification notification;

            if (option == 1)
            {
                notification = SendEmailNotification(user.Email);
                notification.Send(user);
                user.AddNotification(notification);
                Console.WriteLine($"Notification sent to {user.Name} via Email.");
            }
            else if (option == 2)
            {
                notification = SendSMSNotification(user.PhoneNumber);
                notification.Send(user);
                user.AddNotification(notification);
                Console.WriteLine($"Notification sent to {user.Name} via SMS.");
            }
                
        }
    }
}