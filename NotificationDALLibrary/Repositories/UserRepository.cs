using System;
using System.Collections.Generic;
using Npgsql;
using NotificationDALLibrary.Infrastructure;
using NotificationModelLibrary.Models;

namespace NotificationDALLibrary.Repositories
{
    public class UserRepository
    {
        private readonly NpgsqlConnection connection;

        public UserRepository(string connectionString)
        {
            connection=new NpgsqlConnection(connectionString);
        }
        
        public User? Create(User item)
        {
            string createUserCommand = $"insert into users values('{item.Email}','{item.Name}','{item.PhoneNumber}')";
            NpgsqlCommand command = new NpgsqlCommand(createUserCommand,connection);
            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();
                return item;
    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                connection?.Close();
            }
        }

        public List<User>? GetAll()
        {
            string sql = "SELECT email, name, phone_number FROM users ORDER BY name;";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                List<User> users = new List<User>();
                
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Email = reader[0].ToString() ?? string.Empty,
                        Name = reader[1].ToString() ?? string.Empty,
                        PhoneNumber = reader[2].ToString() ?? string.Empty
                    });
                }
                
                return users.Count == 0 ? null : users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                connection?.Close();
            }
        }

        public User? Get(string email)
        {
            string sql = $"SELECT email, name, phone_number FROM users WHERE email = '{email}' LIMIT 1;";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                
                if (!reader.Read())
                {
                    return null;
                }

                return new User
                {
                    Email = reader[0].ToString() ?? string.Empty,
                    Name = reader[1].ToString() ?? string.Empty,
                    PhoneNumber = reader[2].ToString() ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                connection?.Close();
            }
        }

        public User? Update(string email, User item)
        {
            string sql = $"UPDATE users SET name = '{item.Name}', phone_number = '{item.PhoneNumber}' WHERE email = '{email}' RETURNING email;";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                object? updatedEmail = command.ExecuteScalar();
                return updatedEmail == null ? null : item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                connection?.Close();
            }
        }

        public bool Delete(string email)
        {
            string sql = $"DELETE FROM users WHERE email = '{email}';";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
            try
            {
                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection?.Close();
            }
        }
    }
}