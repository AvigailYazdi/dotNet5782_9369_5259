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

        //private object customer;
        /// <summary>
        /// A function that checks if a customer appears in the list 
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        private bool checkC(int id)
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            return customers.Any(cs => cs.Id == id);
        }
        private int indexCustomer(int id)
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            for (int i = 0; i < customers.Count(); i++)
            {
                if (customers[i].Id == id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// A function that adds a customer to the array
        /// </summary>
        /// <param name="c"> the customer to add</param>
        public void AddCustomer(Customer c)
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            if (checkC(c.Id))
                throw new DuplicateIdException(c.Id, "Customer");
            customers.Add(c);
            XmlTools.SaveListToXMLSerializer(customers, customersPath);
        }

        /// <summary>
        /// A function that shows the requested customer
        /// </summary>
        /// <param name="id"> the id of the requested customer</param>
        /// <returns> returns the requested customer</returns>
        public Customer GetCustomer(int id)
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            if (!checkC(id))
                throw new MissingIdException(id, "Customer");
            return customers.Find(c => c.Id == id);
        }

        /// <summary>
        /// A function that updates a coustomer
        /// </summary>
        /// <param name="c">The updated customer</param>
        public void UpdateCustomer(Customer c)
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            int i = indexCustomer(c.Id);
            customers[i] = c;
            XmlTools.SaveListToXMLSerializer(customers, customersPath);
        }

        /// <summary>
        /// A function that showes the list of the customer
        /// </summary>
        /// <returns> returns the list of the customer</returns>
        public IEnumerable<Customer> ListCustomer()
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            return from item in customers
                   select item;
        }

        /// <summary>
        /// A function that deletes a customer from the list
        /// </summary>
        /// <param name="id"> the id of the customer to delete</param>
        public void DeleteCustomer(int id)
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            if (!checkC(id))
                throw new MissingIdException(id, "Customer");
            customers.RemoveAll(c => c.Id == id);
            XmlTools.SaveListToXMLSerializer(customers, customersPath);
        }

        /// <summary>
        /// A function that returns the customers that stand in a condition
        /// </summary>
        /// <param name="predicate"> The condition</param>
        /// <returns>The customers that stand in a condition </returns>
        public IEnumerable<Customer> GetCustomersByPerdicate(Predicate<Customer> predicate)
        {
            List<Customer> customers = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);
            return from item in customers
                   where predicate(item)
                   select item;
        }

    }
}
