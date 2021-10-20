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
        internal static Station[] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];
    }
}
