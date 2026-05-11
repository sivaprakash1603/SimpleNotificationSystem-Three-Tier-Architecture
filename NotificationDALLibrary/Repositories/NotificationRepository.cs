using System;
using System.Collections.Generic;
using Npgsql;
using NotificationDALLibrary.Infrastructure;
using NotificationModelLibrary.Models;

namespace NotificationDALLibrary.Repositories
{
    public class NotificationRepository
    {
        private readonly NpgsqlConnection connection;

        public NotificationRepository(string connectionString)
        {
            connection = new NpgsqlConnection(connectionString);
        }

        public void SaveNotification(Notification notification, string userEmail)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("User email cannot be empty.", nameof(userEmail));

            string notificationType;
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
            string sql = $"INSERT INTO notifications (user_email, message, sent_date, notification_type, to_email, to_phone_number) VALUES ('{userEmail}', '{notification.Message}', '{notification.SentDate}', '{notificationType}', '{toEmail}', '{toPhoneNumber}');";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        public List<Notification> GetAllNotifications()
        {
            string sql = "SELECT user_email, message, sent_date, notification_type, to_email, to_phone_number FROM notifications ORDER BY sent_date DESC;";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                List<Notification> notifications = new List<Notification>();
                
                while (reader.Read())
                {
                    notifications.Add(MapNotification(reader));
                }
                return notifications;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Notification>();
            }
            finally
            {
                connection?.Close();
            }
        }

        public List<Notification> GetNotificationsForUser(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentException("User email cannot be empty.", nameof(userEmail));

            string sql = $"SELECT user_email, message, sent_date, notification_type, to_email, to_phone_number FROM notifications WHERE user_email = '{userEmail}' ORDER BY sent_date DESC;";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                List<Notification> notifications = new List<Notification>();
                
                while (reader.Read())
                {
                    notifications.Add(MapNotification(reader));
                }
                return notifications;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Notification>();
            }
            finally
            {
                connection?.Close();
            }
        }

        public void ClearAllNotifications()
        {
            string sql = "DELETE FROM notifications;";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        public int GetNotificationCount()
        {
            string sql = "SELECT COUNT(*) FROM notifications;";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                object? result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                connection?.Close();
            }
        }

        public List<Notification>? Get(string userEmail)
        {
            List<Notification> notifications = GetNotificationsForUser(userEmail);
            return notifications.Count == 0 ? null : notifications;
        }

        private static Notification MapNotification(NpgsqlDataReader reader)
        {
            string notificationType = reader[3].ToString() ?? string.Empty;
            string message = reader[1].ToString() ?? string.Empty;
            DateTime sentDate = (DateTime)reader[2];
            string userEmail = reader[0].ToString() ?? string.Empty;

            Notification notification = notificationType switch
            {
                "email" => new EmailNotification(reader[4].ToString() ?? string.Empty, message),
                "sms" => new SmsNotification(reader[5].ToString() ?? string.Empty, message),
                _ => throw new InvalidOperationException($"Unknown notification type: {notificationType}")
            };

            notification.SentDate = sentDate;
            notification.UserEmail = userEmail;
            return notification;
        }
    }
}
