using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationModelLibrary.Exceptions;
using NotificationModelLibrary.Interfaces;

namespace NotificationModelLibrary.Models
{
    public abstract class Notification : INotification
    {
        public string Message { get; set; } = string.Empty;
        public DateTime SentDate { get; set; } = DateTime.Now;

        public Notification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Notification message cannot be empty.", nameof(message));
            }

            Message = message;
            SentDate = DateTime.Now;
        }

        public abstract void Send(User user);

        public override string ToString()
        {
            return $"Message: {Message}, SentDate: {SentDate}";
        }
    }
}