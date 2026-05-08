using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationModelLibrary.Models;

namespace NotificationBLLibrary.Interfaces
{
    public interface ISendNotification
    {
        public Notification SendEmailNotification(string email);
        public Notification SendSMSNotification(string phoneNumber);
        public void SendNotificationToUser(User user);
    }
}