using NotificationDALLibrary.Interfaces;
using NotificationModelLibrary.Models;


namespace NotificationDALLibrary.Repositories
{
    public class UserRepository : AbstractRepository<string, User>
    {
        public UserRepository()
        {
            _items = new Dictionary<string, User>();
        }
        
        public User this[string index]
        {   
            get{return _items![index];}
            set{_items![index] = value;}
        }
        public override User? Create(User item)
        {
            if (_items==null||_items.ContainsKey(item.Email))
            {
                return null;
            }
            _items.Add(item.Email, item);
            return item;
        }

    }
}