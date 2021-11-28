using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IDAL;

namespace IBL
{
    public partial class BL : IBL
    {
        private object customer;

        /// <summary>
        /// A function that returns the customer name
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> Returns the name of the customer </returns>
        private string getCustomerName(int id)
        {
            IDAL.DO.Customer c =new IDAL.DO.Customer();
            try
            {
                c = dl.GetCustomer(id);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new  BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return c.Name;
        }
        /// <summary>
        /// A function that adds a customer to the data base
        /// </summary>
        /// <param name="customer"> The customer to add</param>
        public void AddCustomer(BO.Customer customer)
        {
            IDAL.DO.Customer c= new IDAL.DO.Customer();
            c.Id = customer.Id;
            c.Name = customer.Name;
            c.Phone = customer.PhoneNum;
            c.Longitude = customer.Place.Longitude;
            c.Latitude = customer.Place.Latitude;
            try
            {
                dl.AddCustomer(c);
            }
            catch(IDAL.DO.DuplicateIdException ex)
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
            BO.Customer c = new BO.Customer();
            try
            {
                c = GetCustomer(id);
                if (name != "")
                    c.Name = name;
                if (phoneNum != "")
                    c.PhoneNum = phoneNum;
                dl.DeleteCustomer(id);
                AddCustomer(c);
            }
            catch (IDAL.DO.MissingIdException ex)
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
            BO.Customer c = new BO.Customer();
            try
            {
                IDAL.DO.Customer customer = dl.GetCustomer(id);
                c.Id = customer.Id;
                c.Name = customer.Name;
                c.PhoneNum = customer.Phone;
                c.Place.Longitude = customer.Longitude;
                c.Place.Latitude = customer.Latitude;
                c.SendParcel = getSendParcel(id);
                c.GetParcel = getRecievedParcel(id);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return c;
        }
        /// <summary>
        /// A function that returns the list of all the customers
        /// </summary>
        /// <returns> All the customers</returns>
        public IEnumerable<BO.Customer> CustomerList()
        {
            return from item in dl.ListCustomer()
                   select GetCustomer(item.Id);
        }
    }
}
