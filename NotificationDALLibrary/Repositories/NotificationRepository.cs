using System;
using System.Collections.Generic;
using System.Linq;
using NotificationModelLibrary.Models;

namespace NotificationDALLibrary.Repositories
{
    // Repository for storing and retrieving notifications, extends AbstractRepository
    public class NotificationRepository : AbstractRepository<string, List<Notification>>
    {
        private static List<Notification> _allNotifications = new List<Notification>();

        public NotificationRepository()
        {
            if (_items == null)
                _items = new Dictionary<string, List<Notification>>();
        }

        // Saves a notification to the repository and links it to the user
        public void SaveNotification(Notification notification, string userEmail)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("User email cannot be empty.", nameof(userEmail));

            notification.UserEmail = userEmail;
            _allNotifications.Add(notification);

            // Get or create notifications list for this user using inherited Get
            var userNotifications = Get(userEmail);
            if (userNotifications == null)
            {
                userNotifications = new List<Notification>();
                _items![userEmail] = userNotifications;
            }

            userNotifications.Add(notification);
        }

        // Retrieves all saved notifications
        public List<Notification> GetAllNotifications()
        {
            return _allNotifications.ToList();
        }

        // Retrieves notifications for a specific user by email using inherited Get
        public List<Notification> GetNotificationsForUser(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("User email cannot be empty.", nameof(userEmail));

            return Get(userEmail) ?? new List<Notification>();
        }

        // Clears all notifications
        public void ClearAllNotifications()
        {
            _allNotifications.Clear();
            if (_items != null)
                _items.Clear();
        }

        // Gets the total count of stored notifications
        public int GetNotificationCount()
        {
            return _allNotifications.Count;
        }

        // Initialize an empty notification list for a user
        public override List<Notification>? Create(List<Notification> item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_items == null)
                _items = new Dictionary<string, List<Notification>>();

            return item;
        }
    }
}
