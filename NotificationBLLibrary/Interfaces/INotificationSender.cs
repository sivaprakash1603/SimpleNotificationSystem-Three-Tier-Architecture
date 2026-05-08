using NotificationModelLibrary.Models;

namespace NotificationBLLibrary.Interfaces
{
    // Interface to send notifications using polymorphism
    public interface INotificationSender
    {
        // Sends a notification to a user
        void Send(User user, Notification notification);
    }
}
