using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;

namespace DalObject
{
    internal static class DataSource
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
  


        enum Name { Aviad,Shilat,Nitay,Rinat,Itay,Avigail,Yehuda,Eyal,Michal,Talya};
        enum BaseName { Macabim,Yarden};
        enum ModelName { GC126, UI538X,E765,RFT3,H234};

        /// <summary>
        /// A function that initialize the first ten customers in the array
        /// </summary>
        private static void createCustomer()
        {
            for (int i = 0; i < 10; i++)
            {
                Customer c = new Customer()
                {
                    Id = 212435840+i,//rand.Next(100000000, 1000000000),
                    Name = ((Name)i).ToString(),
                    Phone = "0" + rand.Next(50000000, 56000000).ToString()+i,
                    Longitude = (double)rand.Next(317, 336) / 10,
                    Latitude = (double)rand.Next(350, 363) / 10,
                };
                customers.Add(c);
            }
        }

        /// <summary>
        /// A function that initialize the first five drones in the array
        /// </summary>
        private static void createDrone()
        {
            for (int i = 0; i < 5; i++)
            {
                Drone d = new Drone()
                {
                    Id = 450 + i,// rand.Next(1000, 10000);
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Model = ((ModelName)i).ToString()

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
                    Id = 21001+i,//rand.Next(1000, 10000);
                    Name = ((BaseName)i).ToString(),
                    Longitude = (double)rand.Next(317, 336) / 10,
                    Latitude = (double)rand.Next(350, 363) / 10,
                    ChargeSlots = rand.Next(15, 30)
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
                Parcel p = new Parcel();
                p.Id = config.parcelId;
                p.SenderId = rand.Next(212435840, 212435850);//rand.Next(100000000, 1000000000),
                p.TargetId = rand.Next(212435840, 212435850);//rand.Next(100000000, 1000000000),
                while (p.SenderId == p.TargetId)
                {
                    p.TargetId = rand.Next(212435840, 212435850);//rand.Next(100000000, 1000000000),
                }
                p.Weight = (WeightCategories)rand.Next(3);
                p.Priority = (Priorities)rand.Next(3);
                p.Requested = DateTime.Now;
                p.DroneId = 0;
                p.Scheduled = null;
                p.Delivered = null;
                p.PickedUp = null;
                parcels.Add(p);
                config.parcelId++;
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
