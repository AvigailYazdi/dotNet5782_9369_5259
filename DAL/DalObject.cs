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
            p.Id = DataSource.config.parcelId;
            DataSource.parcels[DataSource.config.parcelId++] = p;
        }
        public static void UpdateParcelToDrone(int DroneId, int ParcelId)
        {
            int i = 0;
            for ( ; (i < DataSource.config.counterParcel) && (DataSource.parcels[i].Id != ParcelId); i++){ }
            DataSource.parcels[i].DroneId = DroneId;
            DataSource.parcels[i].Scheduled = DateTime.Now;
            i = 0;
            for (i=0 ; (i < DataSource.config.counterDrone) && (DataSource.drones[i].Id != DroneId); i++) { }
            DataSource.drones[i].Status = DroneStatuses.Delivery;
        }
        public static void UpdateParcelCollect(int ParcelId)
        {
            int i = 0;
            for (; (i < DataSource.config.counterParcel) && (DataSource.parcels[i].Id != ParcelId); i++) { }
            DataSource.parcels[i].PickedUp = DateTime.Now;
        }
        public static void UpdateParcelDelivery(int ParcelId)
        {
            int i = 0;
            for (; (i < DataSource.config.counterParcel) && (DataSource.parcels[i].Id != ParcelId); i++) { }
            DataSource.parcels[i].Delivered = DateTime.Now;
        }
        public static void UpdateChargeDrone(int DroneId, int BaseStationId)
        {
            int i = 0;
            for (; (i < DataSource.config.counterDrone) && (DataSource.drones[i].Id != DroneId); i++) { }
            DataSource.drones[i].Status = DroneStatuses.Maintenance;
            i = 0;
            for(i = 0 ; (i < DataSource.config.counterStation) && (DataSource.stations[i].Id != BaseStationId); i++) { }
            DataSource.stations[i].ChargeSlots--;
            DataSource.droneCharge[DataSource.config.counterDroneCharge++].DroneId = DroneId;
            DataSource.droneCharge[DataSource.config.counterDroneCharge++].StationId = BaseStationId;
        }
        public static void UpdateDischargeDrone(int DroneId)
        {
            int i = 0;
            for (; (i < DataSource.config.counterDrone) && (DataSource.drones[i].Id != DroneId); i++) { }
            DataSource.drones[i].Status = DroneStatuses.Avaliable;
            i = 0;
            for (; (i < DataSource.config.counterDroneCharge) && (DataSource.droneCharge[i].DroneId != DroneId); i++) { }
            DataSource.stations[DataSource.droneCharge[i].StationId].ChargeSlots++;
        }
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
