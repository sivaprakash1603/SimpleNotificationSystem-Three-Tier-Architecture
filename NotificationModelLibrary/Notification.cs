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
        private const int MinMessageLength = 5;
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public string UserEmail { get; set; } = string.Empty;

        public User User { get; set; } = null!;

        protected Notification() { }

        public Notification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Notification message cannot be empty.", nameof(message));
            }

            if (message.Length < MinMessageLength)
            {
                throw new ArgumentException($"Notification message length should be at least {MinMessageLength} characters.", nameof(message));
            }

            Message = message;
            SentDate = DateTime.UtcNow;
        }

        // Send notification to user
        public abstract void Send(User user);

        public override string ToString()
        {
            return $"Message: {Message}, SentDate: {SentDate}";
        }
    }
}