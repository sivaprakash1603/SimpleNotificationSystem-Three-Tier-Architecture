using System;
using NotificationBLLibrary.Interfaces;
using NotificationModelLibrary.Models;

namespace NotificationBLLibrary.Services
{
    // Sends SMS notifications to users
    public class SmsNotificationSender : INotificationSender
    {
        private const int MaxSmsLength = 160;

        public void Send(User user, Notification notification)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            if (!IsValidPhoneNumber(user.PhoneNumber))
                throw new InvalidOperationException($"Invalid phone number for user: {user.PhoneNumber}");

            if (notification.Message.Length > MaxSmsLength)
                throw new InvalidOperationException($"SMS message exceeds maximum length of {MaxSmsLength} characters. Current length: {notification.Message.Length}");

            notification.Send(user);
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
