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
        /// <param name="Id">The id of the drone</param>
        private void checkD(int Id)
        {
            if (!drones.Any(dn => dn.Id == Id))
                throw new MissingIdException(Id, "Drone");
        }
        /// <summary>
        ///  A function that adds a drone to the array
        /// </summary>
        /// <param name="d"> the drone to add </param>
        public void AddDrone(Drone d)
        {
            if (drones.Any(dr => dr.Id == d.Id))
                throw new DuplicateIdException(d.Id, "Drone");
            drones.Add(d);
        }
        /// <summary>
        /// A function that updates a drone into a charge slot of a station
        /// </summary>
        /// <param name="DroneId"> the id of the drone</param>
        /// <param name="BaseStationId"> the id of the base station</param>
        public void UpdateChargeDrone(int DroneId, int BaseStationId)
        {
            checkD(DroneId);
            checkS(BaseStationId);
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].Id == BaseStationId)
                {
                    BaseStation b = stations[i];
                    b.ChargeSlots--;
                    stations[i] = b;
                    break;
                }
            }
            DroneCharge dc = new DroneCharge();
            dc.DroneId = DroneId;
            dc.StationId = BaseStationId;
            droneCharge.Add(dc);
        }
        /// <summary>
        /// A function that discharge a drone from a charge slot of a station
        /// </summary>
        /// <param name="DroneId"> the id of the drone to discharge</param>
        public void UpdateDischargeDrone(int DroneId)
        {
            checkD(DroneId);
            for (int i = 0; i < droneCharge.Count; i++)
            {
                if (droneCharge[i].DroneId == DroneId)
                {
                    DroneCharge d = droneCharge[i];
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
        }
        /// <summary>
        /// A function that shows the requested drone
        /// </summary>
        /// <param name="Id"> the id of the requested drone</param>
        /// <returns> returns the requested drone</returns>
        public Drone GetDrone(int Id)
        {
            checkD(Id);
            return drones.Find(d => d.Id == Id);
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
        /// <param name="d"> The drone to delete </param>
        public void DeleteDrone(int id)
        {
            checkD(id);
            drones.Remove(GetDrone(id));
        }
        public IEnumerable<Drone> GetDronesByPerdicate(Predicate<Drone> predicate)
        {
            return from item in drones
                   where predicate(item)
                   select item;
        }
    }
}
