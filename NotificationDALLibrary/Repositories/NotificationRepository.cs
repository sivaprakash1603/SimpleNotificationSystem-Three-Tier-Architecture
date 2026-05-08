using System;
using System.Collections.Generic;
using System.Linq;
using NotificationModelLibrary.Models;

namespace NotificationDALLibrary.Repositories
{
    // Repository for storing and retrieving notifications
    public class NotificationRepository
    {
        private static List<Notification> _notifications = new List<Notification>();

        // Saves a notification to the repository
        public void SaveNotification(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            _notifications.Add(notification);
        }

        // Retrieves all saved notifications
        public List<Notification> GetAllNotifications()
        {
            return _notifications.ToList();
        }

        // Retrieves notifications for a specific user
        public List<Notification> GetNotificationsForUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return user.Notifications;
        }

        // Clears all notifications
        public void ClearAllNotifications()
        {
            _notifications.Clear();
        }

        // Gets the total count of stored notifications
        public int GetNotificationCount()
        {
            return _notifications.Count;
        }
    }
}
