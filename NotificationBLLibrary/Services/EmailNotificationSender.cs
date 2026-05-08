using System;
using NotificationBLLibrary.Interfaces;
using NotificationModelLibrary.Models;

namespace NotificationBLLibrary.Services
{
    // Sends email notifications to users
    public class EmailNotificationSender : INotificationSender
    {
        public void Send(User user, Notification notification)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            if (!IsValidEmail(user.Email))
                throw new InvalidOperationException($"Invalid email address for user: {user.Email}");

            notification.Send(user);
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
    }
}
