using System;
using System.Collections.Generic;
using Npgsql;
using NotificationModelLibrary.Models;
using NotificationDALLibrary.Contexts;

namespace NotificationDALLibrary.Repositories
{
    public class UserRepository: AbstractRepository<string, User>
    {
        
        public UserRepository() : base()
        {
            _context = new NotificationContext();
        }

        public override User? Create(User item)
        {
            if (Get(item.Email) != null)
            {
                throw new InvalidOperationException($"A user with email '{item.Email}' already exists.");
            }
            return base.Create(item);
        }
    }
}