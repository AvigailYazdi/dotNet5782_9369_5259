using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{
    sealed partial class DalXml:IDal
    {
        /// <summary>
        /// A function that checks if a drone appears in the list
        /// </summary>
        /// <param name="id">The id of the drone</param>
        private bool checkD(int id)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return drones.Any(dn => dn.Id == id);
        }
        private int indexDrone(int id)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            for (int i = 0; i < drones.Count(); i++)
            {
                if (drones[i].Id == id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        ///  A function that adds a drone to the array
        /// </summary>
        /// <param name="d"> the drone to add </param>
        public void AddDrone(Drone d)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (checkD(d.Id))
                throw new DuplicateIdException(d.Id, "Drone");
            drones.Add(d);
            XmlTools.SaveListToXMLSerializer(drones,dronesPath);
        }

        /// <summary>
        /// A function that returns the id of the parcel that the drone is connected to, else -1
        /// </summary>
        /// <param name="id">the id of a derone to check</param>
        /// <returns>the id of the connected parcel, else -1</returns>
        public int GetConnectParcel(int id)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            Parcel pr = new Parcel() { };
            if (checkD(id))
            {
                List<Parcel> parcels = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
                pr = parcels.Find(p => p.DroneId == id && p.Delivered==null);
            }
            if (pr.Id != 0)
                return pr.Id;
            else
                return -1;
        }

        /// <summary>
        /// A function that updates a drone into a charge slot of a station
        /// </summary>
        /// <param name="droneId"> the id of the drone</param>
        /// <param name="baseStationId"> the id of the base station</param>
        public void UpdateChargeDrone(int droneId, int baseStationId)
        {
            List<DroneCharge> dronesCharge = XmlTools.LoadListFromXMLSerializer<DroneCharge>(dronesChargePath);
            if (!checkD(droneId))
                throw new MissingIdException(droneId, "Drone");
            if (!checkS(baseStationId))
                throw new MissingIdException(baseStationId, "Base station");
            List<BaseStation> stations = XmlTools.LoadListFromXMLSerializer<BaseStation>(stationsPath);
            foreach (BaseStation item in stations)
            {
                if (item.Id == baseStationId)
                {
                    BaseStation b = item;
                    b.ChargeSlots--;
                    UpdateStation(b);
                    break;
                }
            }
            DroneCharge dc = new DroneCharge();
            dc.DroneId = droneId;
            dc.StationId = baseStationId;
            dc.insertTime = DateTime.Now;
            dronesCharge.Add(dc);
            XmlTools.SaveListToXMLSerializer(dronesCharge, dronesChargePath);

        }

        /// <summary>
        /// A function that discharge a drone from a charge slot of a station
        /// </summary>
        /// <param name="droneId"> the id of the drone to discharge</param>
        public void UpdateDischargeDrone(int droneId)
        {
            if (!checkD(droneId))
                throw new MissingIdException(droneId, "Drone");
            List<DroneCharge> dronesCharge = XmlTools.LoadListFromXMLSerializer<DroneCharge>(dronesChargePath);
            List<BaseStation> stations = XmlTools.LoadListFromXMLSerializer<BaseStation>(stationsPath);
            foreach (DroneCharge item1 in dronesCharge)
            {
                if (item1.DroneId == droneId)
                {
                    DroneCharge d = item1;
                    foreach (BaseStation item2 in stations)
                    {
                        if (d.StationId == item2.Id)
                        {
                            BaseStation s = item2;
                            s.ChargeSlots++;
                            UpdateStation(s);
                            break;
                        }
                    }
                    break;
                }
            }
            DeleteDroneCharge(droneId);
        }

        /// <summary>
        /// A function that shows the requested drone
        /// </summary>
        /// <param name="id"> the id of the requested drone</param>
        /// <returns> returns the requested drone</returns>
        public Drone GetDrone(int id)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (!checkD(id))
                throw new MissingIdException(id, "Drone");
            return drones.Find(d => d.Id == id);
        }

        /// <summary>
        /// A function that updates a drone
        /// </summary>
        /// <param name="d">The updated drone</param>
        public void UpdateDrone(Drone d)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            int i = indexDrone(d.Id);
            drones[i] = d;
            XmlTools.SaveListToXMLSerializer(drones, dronesPath);
        }

        /// <summary>
        /// A function that showes the list of the drones
        /// </summary>
        /// <returns> returns the list of the drones</returns>
        public IEnumerable<Drone> ListDrone()
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return from item in drones
                   select item;
        }

        /// <summary>
        /// A function that deletes a drone from the list
        /// </summary>
        /// <param name="id"> The id of the drone to delete </param>
        public void DeleteDrone(int id)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (!checkD(id))
                throw new MissingIdException(id, "Drone");
            drones.RemoveAll(d => d.Id == id);
            XmlTools.SaveListToXMLSerializer(drones, dronesPath);
        }

        /// <summary>
        /// A function that returns the drones that stand in a condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        /// <returns>The drones that stand in the condition</returns>
        public IEnumerable<Drone> GetDronesByPerdicate(Predicate<Drone> predicate)
        {
            List<Drone> drones = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return from item in drones
                   where predicate(item)
                   select item;
        }
    }
}
