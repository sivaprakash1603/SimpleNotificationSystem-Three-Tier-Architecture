using System;
using System.Collections.Generic;
using Npgsql;
using NotificationModelLibrary.Models;
using NotificationDALLibrary.Contexts;

namespace NotificationDALLibrary.Repositories
{
    public class NotificationRepository: AbstractRepository<string, Notification> 
    {

        public NotificationRepository()
        {
            _context = new NotificationContext();
        }

        public void SaveNotification(Notification notification, string userEmail)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("User email cannot be empty.", nameof(userEmail));

            string notificationType = string.Empty;
            string? toEmail = null;
            string? toPhoneNumber = null;

            switch (notification)
            {
                case EmailNotification emailNotification:
                    notificationType = "email";
                    toEmail = emailNotification.ToEmail;
                    break;
                case SmsNotification smsNotification:
                    notificationType = "sms";
                    toPhoneNumber = smsNotification.ToPhoneNumber;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported notification type: {notification.GetType().Name}");
            }

            notification.UserEmail = userEmail;
            _context.Add(notification);
            _context.SaveChanges();
        }

        public List<Notification> GetAllNotifications()
        {
            return _context.Set<Notification>().ToList();
        }
               

        public List<Notification> GetNotificationsForUser(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("User email cannot be empty.", nameof(userEmail));

            return _context.Set<Notification>().Where(n => n.UserEmail == userEmail).ToList();
        }

        public void ClearAllNotifications()
        {
            _context.Set<Notification>().RemoveRange(_context.Set<Notification>());
            _context.SaveChanges();
        }

        public int GetNotificationCount()
        {
            return _context.Set<Notification>().Count();
        }
    }
}
