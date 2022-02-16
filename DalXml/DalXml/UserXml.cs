using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{
    sealed partial class DalXml : IDal
    {
        private bool checkU(string password)
        {
            List<User> users = XmlTools.LoadListFromXMLSerializer<User>(usersPath);
            return users.Any(cs => cs.Password == password);
        }
        public void AddUser(User c)
        {
            List<User> users = XmlTools.LoadListFromXMLSerializer<User>(usersPath);
            if (checkU(c.Password))
                throw new DuplicatePasswordException();
            users.Add(c);
            XmlTools.SaveListToXMLSerializer(users, usersPath);
        }
        public User GetUser(string password)
        {
            List<User> users = XmlTools.LoadListFromXMLSerializer<User>(usersPath);
            if (!checkU(password))
                throw new MissingPasswordException();
            return users.Find(c => c.Password == password);
        }
        public IEnumerable<User> ListUser()
        {
            List<User> users = XmlTools.LoadListFromXMLSerializer<User>(usersPath);
            return from item in users
                   select item;
        }
        public IEnumerable<User> GetUsersByPerdicate(Predicate<User> predicate)
        {
            List<User> users = XmlTools.LoadListFromXMLSerializer<User>(usersPath);
            return from item in users
                   where predicate(item)
                   select item;
        }
    }
}
