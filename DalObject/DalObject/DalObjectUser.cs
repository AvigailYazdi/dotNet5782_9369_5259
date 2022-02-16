using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static DalObject.DataSource;
using DO;

using DalApi;

namespace Dal
{
    sealed partial class DalObject : IDal
    {
        private bool checkU(string password)
        {
            return users.Any(cs => cs.Password == password);
        }
        public void AddUser(User c)
        {
            if (checkU(c.Password))
                throw new DuplicatePasswordException();
            users.Add(c);
        }
        public User GetUser(string password)
        {
            if (!checkU(password))
                throw new MissingPasswordException();
            return users.Find(c => c.Password == password);
        }
        public IEnumerable<User> ListUser()
        {
            return from item in users
                   select item;
        }
        public IEnumerable<User> GetUsersByPerdicate(Predicate<User> predicate)
        {
            return from item in users
                   where predicate(item)
                   select item;
        }
    }
}
