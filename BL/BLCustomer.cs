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
        /// <param name="Id"> The id of the customer</param>
        /// <returns> Returns the name of the customer </returns>
        private string GetCustomerName(int Id)
        {
            IDAL.DO.Customer c =new IDAL.DO.Customer();
            try
            {
                c = dl.GetCustomer(Id);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new  BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return c.Name;
        }
        /// <summary>
        /// A function that returns the status of the parcel
        /// </summary>
        /// <param name="p"> The parcel</param>
        /// <returns> Returns the status</returns>
        private BO.ParcelStatus GetParcelStatus(int Id)
        {
            DateTime t = new DateTime();
            IDAL.DO.Parcel p = dl.GetParcel(Id);
            if (p.Delivered != t) // The parcel delivered
                return BO.ParcelStatus.Provided;
            if (p.PickedUp != t) // The parcel Picked up
                return BO.ParcelStatus.PickedUp;
            if (p.Scheduled != t) // The parcel connected
                return BO.ParcelStatus.Connected;
            return BO.ParcelStatus.Created; // The parcel created
        }
        /// <summary>
        /// A function that returns all the parcel the customer has sent
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels</returns>
        private IEnumerable<BO.ParcelAtC> GetSendParcel(int id)
        {
            return from item in dl.GetParcelsByPerdicate(item => item.SenderId == id)
                   let p = dl.GetParcel(item.SenderId)
                   select new BO.ParcelAtC()
                   {
                       Id = p.Id,
                       Weight = (BO.WeightCategories)p.Weight,
                       Priority = (BO.Priorities)p.Priority,
                       Status = GetParcelStatus(p.Id),
                       OtherC = new BO.CustomerInP()
                       {
                           Id = p.Id,
                           Name = GetCustomerName(p.SenderId)
                       }                
                   };                         
        }
        /// <summary>
        /// A function that returns all the parcels the customer has recieved
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels </returns>
        private IEnumerable<BO.ParcelAtC> GetRecievedParcel(int id)
        {
            return from item in dl.GetParcelsByPerdicate(item => item.TargetId == id)
                   let p = dl.GetParcel(item.TargetId)
                   select new BO.ParcelAtC()
                   {
                       Id = p.Id,
                       Weight = (BO.WeightCategories)p.Weight,
                       Priority = (BO.Priorities)p.Priority,
                       Status = GetParcelStatus(p),
                       OtherC = new BO.CustomerInP()
                       {
                           Id = p.Id,
                           Name = GetCustomerName(p.TargetId)
                       }
                   };
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
                c.SendParcel = GetSendParcel(id);
                c.GetParcel = GetRecievedParcel(id);
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
