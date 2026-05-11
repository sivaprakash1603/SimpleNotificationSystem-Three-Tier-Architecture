using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationModelLibrary.Exceptions;
using NotificationModelLibrary.Models;
using NotificationBLLibrary.Interfaces;
using NotificationDALLibrary.Repositories;

namespace NotificationBLLibrary.Services
{
    public class SystemService : ISystemInteract
    {
        private readonly UserRepository _userRepository;
        private readonly NotificationRepository _notificationRepository;

        public SystemService(string connectionString)
        {
            _userRepository = new UserRepository(connectionString);
            _notificationRepository = new NotificationRepository(connectionString);
        }

        private User GetUserOrThrow(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            var user = _userRepository.Get(email);
            if (user == null)
            {
                throw new NotificationException($"No user found with email: {email}");
            }

            return user;
        }

       public void CreateUser()
        {
            Console.WriteLine("Enter user details:");
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;
            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine() ?? string.Empty;
            var user = new User(name, email, phoneNumber);
            if (_userRepository.Create(user) == null)
            {
                throw new NotificationException($"A user with email '{email}' already exists.");
            }
            Console.WriteLine("");
            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine($"User '{name}' created successfully.");
            Console.ResetColor();
        }

        public void ListUsers()
        {
            Console.WriteLine("List of Users:");
            List<User>? users = _userRepository.GetAll();
            if (users == null || !users.Any())
            {   
                Console.ForegroundColor=ConsoleColor.Yellow;
                Console.WriteLine("No users found.");
                Console.ResetColor();
                return;
            }

            var userLines =
                from user in users
                select $"- {user.Name} (Email: {user.Email}, Phone: {user.PhoneNumber})";

            foreach (var line in userLines)
            {
                Console.WriteLine(line);
            }
        }

        public void PrintUserDetails(User user)
        {
            Console.WriteLine($"User Details:\nName: {user.Name}\nEmail: {user.Email}\nPhone: {user.PhoneNumber}");
        }
        public void PrintUserDetailsByEmail()
        {
            Console.Write("Enter the email of the user: ");
            string email = Console.ReadLine() ?? string.Empty;
            PrintUserDetails(GetUserOrThrow(email));
        }

        public void UpdateUser()
        {
            Console.WriteLine("Enter the email of the user to update:");
            string email = Console.ReadLine() ?? string.Empty;
            var user = GetUserOrThrow(email);
            Console.WriteLine("Enter new details (leave blank to keep current value):");
            Console.Write($"Name ({user.Name}): ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write($"Phone Number ({user.PhoneNumber}): ");
            string phoneNumber = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                user.Name = name;
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                user.PhoneNumber = phoneNumber;
            }
            _userRepository.Update(email, user);
            Console.WriteLine($"User details updated successfully.");       
        }
        
        public void DeleteUser()
        {
            Console.WriteLine("Enter the email of the user to delete:");
            string email = Console.ReadLine() ?? string.Empty;
            bool deleted = _userRepository.Delete(GetUserOrThrow(email).Email);
            if (deleted)
            {
                Console.WriteLine($"User with email '{email}' deleted successfully.");
            }
        }

        public User? GetUser()
        {
            Console.Write("Enter the email of the user: ");
            string email = Console.ReadLine() ?? string.Empty;
            return GetUserOrThrow(email);
        }

        public void PrintNotificationsForUser(User user)
        {
            Console.WriteLine($"Notifications for {user.Name}:");
            var notifications = _notificationRepository.GetNotificationsForUser(user.Email);
            if (notifications == null || !notifications.Any())
            {
                Console.WriteLine("No notifications found.");
                return;
            }

            var notificationLines = notifications
                .Select(notification =>
                    $"- {notification.GetType().Name}: {notification.Message} (Sent: {notification.SentDate})");

            foreach (var line in notificationLines)
            {
                Console.WriteLine(line);
            }
        }

    }
}