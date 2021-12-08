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
            internal static double avaliable=0.05;//per km
            internal static double light=0.07;
            internal static double medium=0.1;
            internal static double heavy=0.2;
            internal static double batteryCharge=50;//per hour
        }
        // Initialization of the arrarys
        internal static List<DroneCharge> dronesCharge=new List<DroneCharge>();
        internal static List<Drone> drones=new List<Drone>();
        internal static List<BaseStation> stations=new List<BaseStation>();
        internal static List<Customer> customers=new List<Customer>();
        internal static List<Parcel> parcels=new List<Parcel>();
        private static Random rand = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// A function that initialize the first five drones in the array
        /// </summary>
        private static void createDrone()
        {
            Drone d=new Drone();
            int count = 1;
            for (int i = 0; i < 5; i++)
            {
                d.Id = count++;// rand.Next(1000, 10000);
                d.MaxWeight = (WeightCategories)rand.Next(3);
                d.Model = "";
                drones.Add(d);
            }
        }
        /// <summary>
        /// A function that initialize the first two stations in the array
        /// </summary>
        private static void createBaseStation()
        {
            int count = 1;
            for (int i = 0; i < 2; i++)
            {
                BaseStation s = new BaseStation()
                {
                    Id = count++,//rand.Next(1000, 10000);
                    Name = "",
                    Longitude = (double)rand.Next(293, 336) / 10,
                    Latitude = (double)rand.Next(337, 363) / 10,
                    ChargeSlots = rand.Next(0, 11)
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
                    SenderId = rand.Next(1, 11),//rand.Next(100000000, 1000000000),
                    TargetId = rand.Next(1, 11), //rand.Next(100000000, 1000000000),
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                    DroneId = 0,
                    Scheduled= null,
                    Delivered= null,
                    PickedUp = null,
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
            int count = 1;
            for (int i = 0; i < 10; i++)
            {
                Customer c = new Customer()
                {
                    Id = count++,//rand.Next(100000000, 1000000000),
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
