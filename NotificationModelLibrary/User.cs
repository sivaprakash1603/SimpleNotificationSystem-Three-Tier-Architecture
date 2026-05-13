using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using NotificationModelLibrary.Exceptions;

namespace NotificationModelLibrary.Models
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<Notification> Notifications { get; set; }

        public User() { Notifications = new List<Notification>(); }

        public User(string name, string email, string phoneNumber)
        {
            Notifications = new List<Notification>();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("User name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
            {
                throw new ArgumentException("Email format is invalid.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be empty.", nameof(phoneNumber));
            }

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

    }

}
