using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationModelLibrary.Models;

namespace NotificationBLLibrary.Interfaces
{
    public interface ISystemInteract
    {
        public void CreateUser();
        public void ListUsers();
        public void PrintUserDetails(User user);
        public void PrintUserDetailsByEmail();
        public void UpdateUser();
        public void DeleteUser();
        public User? GetUser();

        public void PrintNotificationsForUser(User user);

    }
}