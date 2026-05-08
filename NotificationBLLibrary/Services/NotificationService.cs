using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationBLLibrary.Interfaces;
using NotificationModelLibrary.Exceptions;
using NotificationModelLibrary.Models;
using NotificationDALLibrary.Repositories;

namespace NotificationBLLibrary.Services
{
    // Handles notification business logic and validation
    public class NotificationService : ISendNotification
    {
        private readonly NotificationRepository _notificationRepository;
        private const int MinMessageLength = 5;
        private const int MaxSmsLength = 160;

        public NotificationService()
        {
            _notificationRepository = new NotificationRepository();
        }

        // Creates and validates an email notification
        public Notification SendEmailNotification(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            if (!IsValidEmail(email))
            {
                throw new ArgumentException("Email format is invalid.", nameof(email));
            }

            Console.WriteLine($"Sending email notification to {email}...");
            Console.WriteLine("Enter the message of the email:");
            string message = Console.ReadLine() ?? string.Empty;

            ValidateMessage(message);

            return new EmailNotification(email, message);
        }

        // Creates and validates an SMS notification
        public Notification SendSMSNotification(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));
            }

            if (!IsValidPhoneNumber(phoneNumber))
            {
                throw new ArgumentException("Phone number format is invalid.", nameof(phoneNumber));
            }

            Console.WriteLine($"Sending SMS notification to {phoneNumber}...");
            Console.WriteLine("Enter the message of the SMS:");
            string message = Console.ReadLine() ?? string.Empty;

            ValidateMessage(message);

            if (message.Length > MaxSmsLength)
            {
                throw new InvalidOperationException($"SMS message exceeds maximum length of {MaxSmsLength} characters. Current length: {message.Length}");
            }

            return new SmsNotification(phoneNumber, message);
        }

        // Orchestrates sending a notification to a user
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
            INotificationSender sender;

            if (option == 1)
            {
                notification = SendEmailNotification(user.Email);
                sender = new EmailNotificationSender();
                sender.Send(user, notification);
                user.AddNotification(notification);
                _notificationRepository.SaveNotification(notification);
                Console.WriteLine($"Notification sent to {user.Name} via Email.");
            }
            else if (option == 2)
            {
                notification = SendSMSNotification(user.PhoneNumber);
                sender = new SmsNotificationSender();
                sender.Send(user, notification);
                user.AddNotification(notification);
                _notificationRepository.SaveNotification(notification);
                Console.WriteLine($"Notification sent to {user.Name} via SMS.");
            }
        }

        // Validates message meets business requirements
        private void ValidateMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty.", nameof(message));
            }

            if (message.Length < MinMessageLength)
            {
                throw new ArgumentException($"Message length should be at least {MinMessageLength} characters.", nameof(message));
            }
        }

        // Validates email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Validates phone number format
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Remove common formatting characters
            string cleaned = phoneNumber.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            // Check if it contains only digits and is reasonably long
            return cleaned.Length >= 10 && cleaned.All(char.IsDigit);
        }
    }
}