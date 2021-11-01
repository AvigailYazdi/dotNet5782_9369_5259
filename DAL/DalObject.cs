using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using static DalObject.DataSource;

namespace DalObject
{
    public class DalObject
    {
        /// <summary>
        /// A function that initialize the arrays.
        /// </summary>
        public DalObject()
        {
            Initialize();
        }
        /// <summary>
        /// A function that changes the number to representation at base 60
        /// </summary>
        /// <param name="num"> the number to change </param>
        /// <returns> returns the number number at base 60 </returns>
        public static string base60(double num)
        {
            string str = Math.Abs((int)num) + "°";
            num = (num - (int)num) * 60.0;
            str += (Math.Abs((int)num) + "'");
            num = (int)((num - (int)num) * 60000.0) / 1000.0;
            str += Math.Abs(num) + "''";
            if (num < 0)
                return str + "S";
            return str + "E";
        }
        /// <summary>
        /// A function that calculates the distance 
        /// </summary>
        /// <param name="lon"> the longtitude</param>
        /// <param name="lat">the lattitude</param>
        /// <param name="choice">the choice between a customer or base station</param>
        /// <param name="id"> the id of base station or customer</param>
        /// <returns>the distance</returns>
        public double distance(double lon, double lat, int choice, int id)
        {
            double dis = 0;
            if (choice == 0)// base station
            {
                int i = searchStation(id);
                dis = Math.Sqrt(Math.Pow(lon - stations[i].Longitude, 2) + Math.Pow(lat - stations[i].Lattitude, 2));
            }
            else // customer
            {
                int i = searchCustomer(id);
                dis = Math.Sqrt(Math.Pow(lon - customers[i].Longitude, 2) + Math.Pow(lat - customers[i].Lattitude, 2));
            }
            return dis;
        }
        /// <summary>
        /// A function that searches the drone that matches the given id 
        /// </summary>
        /// <param name="Id"> the id to look for</param>
        /// <returns> the index of the drone</returns>
        private int searchDrone(int Id)
        {
            int i = 0;
            for (; (i < config.counterDrone) && (drones[i].Id != Id); i++) { }
            return i;
        }
        /// <summary>
        /// A function that searches the parcel that matches the given id 
        /// </summary>
        /// <param name="Id"> the id to look for</param>
        /// <returns> the index of the parcel</returns>
        private int searchParcel(int Id)
        {
            int i = 0;
            for (; (i < config.counterParcel) && (parcels[i].Id != Id); i++) { }
            return i;
        }
        /// <summary>
        /// A function that searches the customer that matches the given id 
        /// </summary>
        /// <param name="Id"> the id to look for</param>
        /// <returns> the index of the customer</returns>
        private int searchCustomer(int Id)
        {
            int i = 0;
            for (; (i < config.counterCustomer) && (customers[i].Id != Id); i++) { }
            return i;
        }
        /// <summary>
        /// A function that searches the station that matches the given id 
        /// </summary>
        /// <param name="Id"> the id to look for</param>
        /// <returns> the index of the station</returns>
        private int searchStation(int Id)
        {
            int i = 0;
            for (; (i < config.counterStation) && (stations[i].Id != Id); i++) { }
            return i;
        }
        /// <summary>
        ///  A function that adds a drone to the array
        /// </summary>
        /// <param name="d"> the drone to add </param>
        public void AddDrone(Drone d)
        {
            drones[config.counterDrone++] = d;
        }
        /// <summary>
        /// A function that adds a station to the array
        /// </summary>
        /// <param name="bs"> the station to add</param>
        public void AddBaseStation(BaseStation bs)
        {
            stations[config.counterStation++] = bs;
        }
        /// <summary>
        /// A function that adds a customer to the array
        /// </summary>
        /// <param name="c"> the customer to add</param>
        public void AddCustomer(Customer c)
        {
            customers[config.counterCustomer++] = c;
        }
        /// <summary>
        /// A function that adds a parcel to the array
        /// </summary>
        /// <param name="p"> the parcel to add</param>
        public void AddParcel(Parcel p)
        {
            p.Id = config.parcelId++;
            parcels[config.counterParcel++] = p;
        }
        /// <summary>
        /// A function that connects between a parcel and a drone
        /// </summary>
        /// <param name="DroneId"> the id of the requested drone</param>
        /// <param name="ParcelId">the id of the requested parcel</param>
        public void UpdateParcelToDrone(int DroneId, int ParcelId)
        {
            int i = searchParcel(ParcelId);
            parcels[i].DroneId = DroneId;
            parcels[i].Scheduled = DateTime.Now;
            drones[searchDrone(DroneId)].Status = DroneStatuses.Delivery;
        }
        /// <summary>
        /// A function that updates the time of picking up the parcel
        /// </summary>
        /// <param name="ParcelId"> the id of parcel that picked up</param>
        public void UpdateParcelCollect(int ParcelId)
        {
            parcels[searchParcel(ParcelId)].PickedUp = DateTime.Now;
        }
        /// <summary>
        /// A function that updates the time of parcel delivery
        /// </summary>
        /// <param name="ParcelId"> the id of the parcel </param>
        public void UpdateParcelDelivery(int ParcelId)
        {
            parcels[searchParcel(ParcelId)].Delivered = DateTime.Now;
        }
        /// <summary>
        /// A function that updates a drone into a charge slot of a station
        /// </summary>
        /// <param name="DroneId"> the id of the drone</param>
        /// <param name="BaseStationId"> the id of the base station</param>
        public void UpdateChargeDrone(int DroneId, int BaseStationId)
        {
            drones[searchDrone(DroneId)].Status = DroneStatuses.Maintenance;
            stations[searchStation(BaseStationId)].ChargeSlots--;
            droneCharge[config.counterDroneCharge].DroneId = DroneId;
            droneCharge[config.counterDroneCharge++].StationId = BaseStationId;
        }
        /// <summary>
        /// A function that discharge a drone from a charge slot of a station
        /// </summary>
        /// <param name="DroneId"> the id of the drone to discharge</param>
        public void UpdateDischargeDrone(int DroneId)
        {
            drones[searchDrone(DroneId)].Status = DroneStatuses.Avaliable;
            int i = 0;
            for (; (i < config.counterDroneCharge) && (droneCharge[i].DroneId != DroneId); i++) { }
            stations[searchStation(droneCharge[i].StationId)].ChargeSlots++;
        }
        /// <summary>
        /// A function that shows the requested station
        /// </summary>
        /// <param name="Id"> the id of the requested station</param>
        /// <returns> returns the requested station</returns>
        public BaseStation ViewBaseStation(int Id)
        {
            return stations[searchStation(Id)];
        }
        /// <summary>
        /// A function that shows the requested drone
        /// </summary>
        /// <param name="Id"> the id of the requested drone</param>
        /// <returns> returns the requested drone</returns>
        public Drone ViewDrone(int Id)
        {
            return drones[searchDrone(Id)];
        }
        /// <summary>
        /// A function that shows the requested customer
        /// </summary>
        /// <param name="Id"> the id of the requested customer</param>
        /// <returns> returns the requested customer</returns>
        public Customer ViewCustomer(int Id)
        {
            return customers[searchCustomer(Id)];
        }
        /// <summary>
        /// A function that shows the requested parcel
        /// </summary>
        /// <param name="Id"> the id of the requested parcel</param>
        /// <returns> returns the requested parcel</returns>
        public Parcel ViewParcel(int Id)
        {
            return parcels[searchParcel(Id)];
        }
        /// <summary>
        /// A function that shows the list of the stations
        /// </summary>
        /// <returns> returns the list of the stations</returns>
        public BaseStation[] ListBaseStation()
        {
            BaseStation[] temp = new BaseStation[config.counterStation]; 
            for (int i = 0; i < config.counterStation; i++)
            {
                temp[i] = stations[i];
            }
            return temp;
        }
        /// <summary>
        /// A function that showes the list of the drones
        /// </summary>
        /// <returns> returns the list of the drones</returns>
        public Drone[] ListDrone()
        {
            Drone[] temp = new Drone[config.counterDrone];
            for (int i = 0; i < config.counterDrone; i++)
            {
                temp[i] = drones[i];
            }
            return temp;
        }
        /// <summary>
        /// A function that showes the list of the customer
        /// </summary>
        /// <returns> returns the list of the customer</returns>
        public Customer[] ListCustomer()
        {
            Customer[] temp = new Customer[config.counterCustomer];
            for (int i = 0; i < config.counterCustomer; i++)
            {
                temp[i] = customers[i];
            }
            return temp;
        }
        /// <summary>
        /// A function that showes the list of the parcels
        /// </summary>
        /// <returns> returns the list of the parcels</returns>
        public Parcel[] ListParcel()
        {
            Parcel[] temp = new Parcel[config.counterParcel];
            for (int i = 0; i < config.counterParcel; i++)
            {
                temp[i] = parcels[i];
            }
            return temp;
        }
        /// <summary>
        /// A function that showes the list of the not connected parcels
        /// </summary>
        /// <returns> returns the list of the not connected parcels</returns>
        public Parcel[] ListNotConnected()
        {
            int counter = 0;
            for (int i = 0; i < config.counterParcel; i++)
            {
                if (parcels[i].DroneId == 0) 
                    counter++;
            }
            Parcel[] temp = new Parcel[counter];
            int j = 0;
            for (int i = 0; i < config.counterParcel; i++)
            {
                if (parcels[i].DroneId == 0)
                    temp[j++] = parcels[i];
            }
            return temp;
        }
        /// <summary>
        /// A function that showes the list of the stations with avaliable slots
        /// </summary>
        /// <returns> returns the list of the stations with avaliable slots</returns>
        public BaseStation[] ListAvaliableSlots()
        {
            int counter = 0;
            for (int i = 0; i < config.counterStation; i++)
            {
                if (stations[i].ChargeSlots != 0)
                    counter++;
            }
            BaseStation[] temp = new BaseStation[counter];
            int j = 0;
            for (int i = 0; i < config.counterStation; i++)
            {
                if (stations[i].ChargeSlots != 0)
                    temp[j++] = stations[i];
            }
            return temp;
        }
    }
}
