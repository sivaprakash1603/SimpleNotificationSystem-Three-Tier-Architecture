using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationBLLibrary.Interfaces;
using NotificationBLLibrary.Services;
using NotificationModelLibrary.Exceptions;
using NotificationModelLibrary.Models;

namespace NotificationSystem
{
    internal class Program
    {
        ISendNotification SendNotification;
        ISystemInteract SystemInteract;

        public Program()
        {
            SendNotification = new NotificationService();
            SystemInteract = new SystemService();
        }

        void StartSystem()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Welcome to the Notification System!");
                    Console.ResetColor();
                    Console.WriteLine("\nMenu:");
                    Console.WriteLine("1. Add User");
                    Console.WriteLine("2. List Users");
                    Console.WriteLine("3. Update User");
                    Console.WriteLine("4. Delete User");
                    Console.WriteLine("5. Send Notification to User");
                    Console.WriteLine("6. List Notifications for User");
                    Console.WriteLine("7. Exit");
                    Console.Write("Choose an option: ");

                    string choice = Console.ReadLine() ?? string.Empty;
                    Console.Clear();
                    switch (choice)
                    {
                        case "1":
                            SystemInteract.CreateUser();
                            break;
                        case "2":
                            SystemInteract.ListUsers();
                            break;
                        case "3":
                            SystemInteract.UpdateUser();
                            break;
                        case "4":
                            SystemInteract.DeleteUser();
                            break;
                        case "5":
                            Console.WriteLine("User List");
                            SystemInteract.ListUsers();
                            User? user = SystemInteract.GetUser();
                            SendNotification.SendNotificationToUser(user!);
                            break;
                        case "6":
                            Console.WriteLine("User List:");
                            SystemInteract.ListUsers();
                            User? user1 = SystemInteract.GetUser();
                            SystemInteract.PrintNotificationsForUser(user1!);
                            break;
                        case "7":
                            Console.WriteLine("Exiting the Appllication");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (NotificationException ex)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ResetColor();
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine($"Invalid input: {ex.Message}");
                    Console.ResetColor();
                }
                catch (InvalidOperationException ex)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine($"Operation error: {ex.Message}");
                    Console.ResetColor();
                }
                catch (FormatException ex)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine($"Format error: {ex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
        static void Main(string[] args)
        {
            new Program().StartSystem();
        }
    }
}