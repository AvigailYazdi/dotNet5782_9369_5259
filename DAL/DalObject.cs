using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        public static void AddDrone(Drone d)
        {
            DataSource.drones[DataSource.config.counterDrone++] = d;
        }
        public static void AddBaseStation(BaseStation bs)
        {
            DataSource.stations[DataSource.config.counterStation++] = bs;
        }
        public static void AddCustomer(Customer c)
        {
            DataSource.customers[DataSource.config.counterCustomer++] = c;
        }
        public static void AddParcel(Parcel p)
        {
            DataSource.parcels[DataSource.config.counterParcel++] = p;
        }
        // updates


        public static BaseStation ViewBaseStation(int Id)
        {
            return DataSource.stations[Id];
        }
        public static Drone ViewDrone(int Id)
        {
            return DataSource.drones[Id];
        }
        public static Customer ViewCustomer(int Id)
        {
            return DataSource.customers[Id];
        }
        public static Parcel ViewParcel(int Id)
        {
            return DataSource.parcels[Id];
        }

        public static BaseStation[] ListBaseStation()
        {
            BaseStation[] temp = new BaseStation[DataSource.config.counterStation]; 
            for (int i = 0; i < DataSource.config.counterStation; i++)
            {
                temp[i] = DataSource.stations[i];
            }
            return temp;
        }
        public static Drone[] ListDrone()
        {
            Drone[] temp = new Drone[DataSource.config.counterDrone];
            for (int i = 0; i < DataSource.config.counterDrone; i++)
            {
                temp[i] = DataSource.drones[i];
            }
            return temp;
        }
        public static Customer[] ListCustomer()
        {
            Customer[] temp = new Customer[DataSource.config.counterCustomer];
            for (int i = 0; i < DataSource.config.counterCustomer; i++)
            {
                temp[i] = DataSource.customers[i];
            }
            return temp;
        }
        public static Parcel[] ListParcel()
        {
            Parcel[] temp = new Parcel[DataSource.config.counterParcel];
            for (int i = 0; i < DataSource.config.counterParcel; i++)
            {
                temp[i] = DataSource.parcels[i];
            }
            return temp;
        }
        public static Parcel[] ListNotConnected()
        {
            int counter = 0;
            for (int i = 0; i < DataSource.config.counterParcel; i++)
            {
                if (DataSource.parcels[i].DroneId == 0) 
                    counter++;
            }
            Parcel[] temp = new Parcel[counter];
            for (int i = 0; i < DataSource.config.counterParcel; i++)
            {
                if (DataSource.parcels[i].DroneId == 0)
                    temp[i] = DataSource.parcels[i];
            }
            return temp;
        }
        public static BaseStation[] ListAvaliableSlots()
        {
            int counter = 0;
            for (int i = 0; i < DataSource.config.counterStation; i++)
            {
                if (DataSource.stations[i].ChargeSlots != 0)
                    counter++;
            }
            BaseStation[] temp = new BaseStation[counter];
            for (int i = 0; i < DataSource.config.counterStation; i++)
            {
                if (DataSource.stations[i].ChargeSlots != 0)
                    temp[i] = DataSource.stations[i];
            }
            return temp;
        }
    }
}
