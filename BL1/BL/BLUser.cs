using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using DalApi;

namespace BL
{
    sealed partial class BL : IBL
    {
        private object user;
        public void AddUser(BO.User c)
        {
            DO.User doUser = new DO.User()
            {
                Name=c.Name,
                Password=c.Password,
                UserRole=(DO.Role)c.UserRole
            };
            try
            {
                dl.AddUser(doUser);
            }
            catch (DO.DuplicatePasswordException ex)
            {
                throw new BO.DuplicatePasswordException(ex.Message);
            }
        }
        public BO.User GetUser(string password)
        {
            DO.User doUser;
            try
            {
                doUser = dl.GetUser(password);
                return new BO.User()
                {
                    Name = doUser.Name,
                    Password = doUser.Password,
                    UserRole = (BO.Role)doUser.UserRole
                };
            }
            catch (DO.MissingPasswordException ex)
            {
                throw new BO.MissingPasswordException(ex.Message);
            }
        }
        public IEnumerable<BO.User> ListUser()
        {
            return from item in dl.ListUser()
                   let u = GetUser(item.Password)
                   select new BO.User()
                   {
                       Name = u.Name,
                       Password= u.Password,
                       UserRole=(BO.Role)u.UserRole
                   };
        }
    }
}
