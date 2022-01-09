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
        private object customer;

        /// <summary>
        /// A function that returns the customer name
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> Returns the name of the customer </returns>
        private string getCustomerName(int id)
        {
            DO.Customer customerDo;
            try
            {
                customerDo = dl.GetCustomer(id);
            }
            catch (DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return customerDo.Name;
        }
        /// <summary>
        /// A function that adds a customer to the data base
        /// </summary>
        /// <param name="customerBo"> The customer to add</param>
        public void AddCustomer(BO.Customer customerBo)
        {
            DO.Customer customerDo = new DO.Customer()
            {
                Id = customerBo.Id,
                Name = customerBo.Name,
                Phone = customerBo.PhoneNum,
                Longitude = customerBo.Place.Longitude,
                Latitude = customerBo.Place.Latitude
            };
            try
            {
                dl.AddCustomer(customerDo);
            }
            catch(DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that updates the name\ phone number of a customer
        /// </summary>
        /// <param name="id"> The id of the customer to update</param>
        /// <param name="name"> The new name</param>
        /// <param name="phoneNum"> The new phone number</param>
        public void UpdateCustomer(int id, string name, string phoneNum)
        {
            try
            {
                DO.Customer customerDo = dl.GetCustomer(id);
                if (name != "")
                    customerDo.Name = name;
                if (phoneNum != "")
                    customerDo.Phone = phoneNum;
                dl.UpdateCustomer(customerDo);
            }
            catch (DO.MissingIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that returns a requested customer
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The customer </returns>
        public BO.Customer GetCustomer(int id)
        {
            try
            {
                DO.Customer customerDo = dl.GetCustomer(id);
                BO.Customer customerBo = new BO.Customer()
                {
                    Id = customerDo.Id,
                    Name = customerDo.Name,
                    PhoneNum = customerDo.Phone,
                    Place = new BO.Location() { Longitude = customerDo.Longitude, Latitude = customerDo.Latitude },
                    SendParcel = getSendParcel(id),
                    GetParcel = getRecievedParcel(id)
                };
                return customerBo;
            }
            catch (DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that returns the list of all the customers
        /// </summary>
        /// <returns> All the customers</returns>
        public IEnumerable<BO.CustomerToL> CustomerList()
        {
            return from item in dl.ListCustomer()
                   let c = GetCustomer(item.Id)
                   select new BO.CustomerToL()
                   {
                       Id = c.Id,
                       Name= c.Name,
                       PhoneNum= c.PhoneNum,
                       NumArrived= getSentAndProviededParcels(c.Id).Count(),
                       NumSend= getSentAndNotProviededParcels(c.Id).Count(),
                       NumGot= c.GetParcel.Count(),
                       NumOnWay= getOnWayParcels(c.Id).Count()
                   };
        }

        public BO.CustomerToL getCustomerToL(int id)
        {
            BO.Customer c = GetCustomer(id);
            return new BO.CustomerToL()
                   {
                       Id = c.Id,
                       Name = c.Name,
                       PhoneNum = c.PhoneNum,
                       NumArrived = getSentAndProviededParcels(c.Id).Count(),
                       NumSend = getSentAndNotProviededParcels(c.Id).Count(),
                       NumGot = c.GetParcel.Count(),
                       NumOnWay = getOnWayParcels(c.Id).Count()
                   };
        }
    }
}
