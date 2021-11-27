using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using System.Collections;
using static DalObject.DataSource;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        private object customer;
        /// <summary>
        /// A function that checks if a customer appears in the list 
        /// </summary>
        /// <param name="Id"> The id of the customer</param>
        private void checkC(int Id)
        {
            if (!customers.Any(cs => cs.Id == Id))
                throw new MissingIdException(Id, "Customer");
        }
        /// <summary>
        /// A function that adds a customer to the array
        /// </summary>
        /// <param name="c"> the customer to add</param>
        public void AddCustomer(Customer c)
        {
            if (customers.Any(cs => cs.Id == c.Id))
                throw new DuplicateIdException(c.Id, "Customer");
            customers.Add(c);
        }
        /// <summary>
        /// A function that shows the requested customer
        /// </summary>
        /// <param name="Id"> the id of the requested customer</param>
        /// <returns> returns the requested customer</returns>
        public Customer GetCustomer(int Id)
        {
            checkC(Id);
            return customers.Find(c => c.Id == Id);
        }
        /// <summary>
        /// A function that showes the list of the customer
        /// </summary>
        /// <returns> returns the list of the customer</returns>
        public IEnumerable<Customer> ListCustomer()
        {
            return from item in customers
                   select item;
        }
        /// <summary>
        /// A function that deletes a customer from the list
        /// </summary>
        /// <param name="c"> the customer to delete</param>
        public void DeleteCustomer(int Id)
        {
            checkC(Id);
            customers.Remove(GetCustomer(Id));
        }
        public IEnumerable<Customer> GetCustomersByPerdicate(Predicate<Customer> predicate)
        {
            return from item in customers
                   where predicate(item)
                   select item;
        }
    }
}
