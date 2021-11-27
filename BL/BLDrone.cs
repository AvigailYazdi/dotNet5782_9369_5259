using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;

namespace IBL
{
    public partial class BL: IBL
    {
        private object drone;
        /// <summary>
        /// A function that adds a drone to the data base
        /// </summary>
        /// <param name="drone"> The drone to add</param>
        public void AddDrone(BO.Drone drone)
        {
            IDAL.DO.Drone d= new IDAL.DO.Drone();
            d.Id = drone.Id;
            d.Model = drone.Model;
            d.MaxWeight = (IDAL.DO.WeightCategories)drone.Weight;
            try
            {
                dl.AddDrone(d);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
            BO.DroneToL droneToList = new BO.DroneToL();
            droneToList.Id= drone.Id;
            droneToList.Model= drone.Model;
            droneToList.Weight= drone.Weight;
            droneToList.Battery = rand.NextDouble()+rand.Next(20,39);
            droneToList.Status = BO.DroneStatus.Maintenance;
            droneToList.CurrentPlace = drone.CurrentPlace;
            DList.Add(droneToList);
        }
        public void UpdateDroneName(int id, string name)
        {
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            try
            {
                d = dl.GetDrone(id);
                d.Id = id;
                d.Model = name;
                dl.DeleteDrone(id);
                dl.AddDrone(d);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
            BO.DroneToL droneToList = DList.Find(dr => dr.Id == id);
            DList.Remove(droneToList);
            droneToList.Model = name;
            DList.Add(droneToList);
        }
        public void UpdateDroneToCharge(int id)
        {
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            try
            {
                d = dl.GetDrone(id);
                if()
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
        }
        public void UpdateDisChargeDrone(int id, double time)
        {

        }
        public BO.Drone GetDrone(int id)
        {

        }
        public IEnumerable<BO.Drone> DroneList()
        {

        }
    }
}
