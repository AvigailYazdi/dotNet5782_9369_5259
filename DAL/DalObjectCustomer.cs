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
        /// <param name="id"> The id of the customer</param>
        private bool checkC(int id)
        {
            return customers.Any(cs => cs.Id == id);
        }
        /////////////////////////////////////////////update
        /// <summary>
        /// A function that adds a customer to the array
        /// </summary>
        /// <param name="c"> the customer to add</param>
        public void AddCustomer(Customer c)
        {
            if (checkC(c.Id))
                throw new DuplicateIdException(c.Id, "Customer");
            customers.Add(c);
        }
        /// <summary>
        /// A function that shows the requested customer
        /// </summary>
        /// <param name="id"> the id of the requested customer</param>
        /// <returns> returns the requested customer</returns>
        public Customer GetCustomer(int id)
        {
            if(!checkC(id))
                throw new MissingIdException(id, "Customer");
            return customers.Find(c => c.Id == id);
        }
        /// <summary>
        /// A function that updates a coustomer
        /// </summary>
        /// <param name="c">The updated customer</param>
        public void UpdateCustomer(Customer c)
        {
            DeleteCustomer(c.Id);
            AddCustomer(c);
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
        /// <param name="id"> the id of the customer to delete</param>
        public void DeleteCustomer(int id)
        {
            if(!checkC(id))
                throw new MissingIdException(id, "Customer");
            customers.RemoveAll(c=>c.Id==id);
        }
        /// <summary>
        /// A function that returns the customers that stand in a condition
        /// </summary>
        /// <param name="predicate"> The condition</param>
        /// <returns>The customers that stand in a condition </returns>
        public IEnumerable<Customer> GetCustomersByPerdicate(Predicate<Customer> predicate)
        {
            return from item in customers
                   where predicate(item)
                   select item;
        }
    }
}
