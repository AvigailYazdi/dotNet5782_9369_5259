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
        internal class config
        {
            internal static int counterDrone = 0;
            internal static int counterStation = 0;
            internal static int counterParcel = 0;
            internal static int counterCustomer = 0;
            //מספר רץ לחבילה
        }
        internal static Drone[] drones = new Drone[10];
        internal static BaseStation[] stations = new BaseStation[5];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];
        private static Random rand = new Random(DateTime.Now.Millisecond);
        private static void createDrone()
        {
            for (int i = 0; i < 5; i++)
            {
                drones[i] = new Drone()
                {
                    Id = rand.Next(1000, 10000),
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3),
                    Battery = 100,
                    Model = ""
                };
                config.counterDrone++;
            }
        }
        private static void createBaseStation()
        {
            for (int i = 0; i < 2; i++)
            {
                stations[i] = new BaseStation()
                {
                    Id = rand.Next(1000, 10000),
                    Name = "",
                    Longitude= (double)rand.Next(293, 336)/10,
                    Lattitude= (double)rand.Next(337, 363) / 10,
                    ChargeSlots= rand.Next(0, 11)
                };
                config.counterStation++;
            }
        }
        private static void createParcel()
        {
            for (int i = 0; i < 10; i++)
            {
                parcels[i] = new Parcel()
                {
                    Id = rand.Next(1, 300),
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
                config.counterParcel++;
            }
        }
        private static void createCustomer()
        {
            for (int i = 0; i < 10; i++)
            {
                customers[i] = new Customer()
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = "",
                    Phone = "",
                    Longitude = (double)rand.Next(293, 336) / 10,
                    Lattitude = (double)rand.Next(337, 363) / 10,
                };
                config.counterCustomer++;
            }
        }
        public static void Initialize()
        {
            createBaseStation();
            createCustomer();
            createDrone();
            createParcel();
        }
    }
}
