using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;

namespace DalObject
{
    class DataSource
    {
        /// <summary>
        /// A class of counters to the different arrays.
        /// </summary>
        internal class config
        { 
            internal static int parcelId = 1;


        }
        // Initialization of the arrarys
        internal static List<DroneCharge> droneCharge;
        internal static List<Drone> drones;
        internal static List<BaseStation> stations;
        internal static List<Customer> customers;
        internal static List<Parcel> parcels;
        private static Random rand = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// A function that initialize the first five drones in the array
        /// </summary>
        private static void createDrone()
        {
            for (int i = 0; i < 5; i++)
            {
                Drone d = new Drone()
                {
                    Id = rand.Next(1000, 10000),
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Model = ""
                };
                drones.Add(d);
            }
        }
        /// <summary>
        /// A function that initialize the first two stations in the array
        /// </summary>
        private static void createBaseStation()
        {
            for (int i = 0; i < 2; i++)
            {
                BaseStation s = new BaseStation()
                {
                    Id = rand.Next(1000, 10000),
                    Name = "",
                    Longitude= (double)rand.Next(293, 336)/10,
                    Latitude= (double)rand.Next(337, 363) / 10,
                    ChargeSlots= rand.Next(0, 11)
                };
                stations.Add(s);
            }
        }
        /// <summary>
        /// A function that initialize the first ten parcels in the array
        /// </summary>
        private static void createParcel()
        {
            for (int i = 0; i < 10; i++)
            {
                Parcel p = new Parcel()
                {
                    Id = config.parcelId,
                    SenderId = rand.Next(100000000, 1000000000),
                    TargetId = rand.Next(100000000, 1000000000),
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                    DroneId = 0,
                    Scheduled= DateTime.Now,
                    Delivered= DateTime.Now,
                    PickedUp = DateTime.Now,
                };
                parcels.Add(p);
                config.parcelId++;
            }
        }
        /// <summary>
        /// A function that initialize the first ten customers in the array
        /// </summary>
        private static void createCustomer()
        {
            for (int i = 0; i < 10; i++)
            {
                Customer c = new Customer()
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = "",
                    Phone = "",
                    Longitude = (double)rand.Next(293, 336) / 10,
                    Latitude = (double)rand.Next(337, 363) / 10,
                };
                customers.Add(c);
            }
        }
        /// <summary>
        /// A function that initialize all the arrays
        /// </summary>
        public static void Initialize()
        {
            createBaseStation();
            createCustomer();
            createDrone();
            createParcel();
        }
    }
}
