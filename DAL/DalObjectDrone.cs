using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using static DalObject.DataSource;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        private object drone;
        /// <summary>
        /// A function that initialize the arrays.
        /// </summary>
        public DalObject()
        {
            Initialize();
        }

        /// <summary>
        /// A function that checks if a drone appears in the list
        /// </summary>
        /// <param name="id">The id of the drone</param>
        private bool checkD(int id)
        {
            return drones.Any(dn => dn.Id == id);
        }

        /// <summary>
        ///  A function that adds a drone to the array
        /// </summary>
        /// <param name="d"> the drone to add </param>
        public void AddDrone(Drone d)
        {
            if (checkD(d.Id))
                throw new DuplicateIdException(d.Id, "Drone");
            drones.Add(d);
        }

        /// <summary>
        /// A function that returns the id of the parcel that the drone is connected to, else -1
        /// </summary>
        /// <param name="id">the id of a derone to check</param>
        /// <returns>the id of the connected parcel, else -1</returns>
        public int GetConnectParcel(int id)
        {
            Parcel pr=new Parcel() { };
            if (checkD(id))
                pr = parcels.Find(p => p.DroneId == id);
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
            if(!checkD(droneId))
                throw new MissingIdException(droneId, "Drone");
            if(!checkS(baseStationId))
                throw new MissingIdException(baseStationId, "Base station");
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].Id == baseStationId)
                {
                    BaseStation b = stations[i];
                    b.ChargeSlots--;
                    stations[i] = b;
                    break;
                }
            }
            DroneCharge dc = new DroneCharge();
            dc.DroneId = droneId;
            dc.StationId = baseStationId;
            dronesCharge.Add(dc);
        }

        /// <summary>
        /// A function that discharge a drone from a charge slot of a station
        /// </summary>
        /// <param name="droneId"> the id of the drone to discharge</param>
        public void UpdateDischargeDrone(int droneId)
        {
            if(!checkD(droneId))
                throw new MissingIdException(droneId, "Drone");
            for (int i = 0; i < dronesCharge.Count; i++)
            {
                if (dronesCharge[i].DroneId == droneId)
                {
                    DroneCharge d = dronesCharge[i];
                    for (int j = 0; j < stations.Count; j++)
                    {
                        if (d.StationId == stations[j].Id)
                        {
                            BaseStation s = stations[j];
                            s.ChargeSlots++;
                            stations[j] = s;
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
            if(!checkD(id))
                throw new MissingIdException(id, "Drone");
            return drones.Find(d => d.Id == id);
        }

        /// <summary>
        /// A function that updates a drone
        /// </summary>
        /// <param name="d">The updated drone</param>
        public void UpdateDrone(Drone d)
        {
            DeleteDrone(d.Id);
            AddDrone(d);
        }

        /// <summary>
        /// A function that showes the list of the drones
        /// </summary>
        /// <returns> returns the list of the drones</returns>
        public IEnumerable<Drone> ListDrone()
        {
            return from item in drones
                   select item;
        }

        /// <summary>
        /// A function that deletes a drone from the list
        /// </summary>
        /// <param name="id"> The id of the drone to delete </param>
        public void DeleteDrone(int id)
        {
            if(!checkD(id))
                throw new MissingIdException(id, "Drone");
            drones.RemoveAll(d=>d.Id==id);
        }

        /// <summary>
        /// A function that returns the drones that stand in a condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        /// <returns>The drones that stand in the condition</returns>
        public IEnumerable<Drone> GetDronesByPerdicate(Predicate<Drone> predicate)
        {
            return from item in drones
                   where predicate(item)
                   select item;
        }
    }
}
