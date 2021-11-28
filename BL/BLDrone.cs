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
        public void AddDrone(BO.Drone drone, int stationId)
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
            BO.BaseStation b = GetStation(num);
            BO.DroneToL droneToList = new BO.DroneToL();
            droneToList.Id= drone.Id;
            droneToList.Model= drone.Model;
            droneToList.Weight= drone.Weight;
            droneToList.Battery = rand.NextDouble()+rand.Next(20,40);
            droneToList.Status = BO.DroneStatus.Maintenance;
            droneToList.CurrentPlace = new BO.Location() { Longitude= b.Place.Longitude, Latitude= b.Place.Latitude };
            dl.UpdateChargeDrone(drone.Id, stationId);
            dList.Add(droneToList);
        }
        /// <summary>
        /// A function that sets the name of a drone
        /// </summary>
        /// <param name="id">the drone id to change</param>
        /// <param name="name">the name to change to</param>
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
            BO.DroneToL droneToList = dList.Find(dr => dr.Id == id);
            dList.Remove(droneToList);
            droneToList.Model = name;
            dList.Add(droneToList);
        }
        /// <summary>
        /// A function that put a drone in charge
        /// </summary>
        /// <param name="id">the id of the drone to charge</param>
        public void UpdateDroneToCharge(int id)
        {
            BO.Drone d = new BO.Drone();
            try
            {
                d = GetDrone(id);
                if (d.Status == BO.DroneStatus.Avaliable)
                {
                    IDAL.DO.BaseStation b = shortDis(d.CurrentPlace.Longitude, d.CurrentPlace.Latitude);
                    if (GetMinBattery() > d.Battery)
                        throw new BO.NotEnoughBatteryException(id);
                    dl.UpdateChargeDrone(id, b.Id);
                    BO.DroneToL droneToList = new BO.DroneToL();
                    droneToList = GetDroneToL(id);
                    dList.Remove(droneToList);
                    droneToList.Status = BO.DroneStatus.Maintenance;
                    droneToList.Battery -= GetMinBattery();
                    droneToList.CurrentPlace = new BO.Location() { Longitude = b.Longitude, Latitude = b.Latitude };
                    dList.Add(droneToList);
                }
                else
                    throw new BO.NotAvaliableDroneException(id);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that discharges a drone
        /// </summary>
        /// <param name="id">the id of a drone to discharge</param>
        /// <param name="time">the time that the drone was in charging</param>
        public void UpdateDisChargeDrone(int id, double time)
        {
            BO.Drone d = new BO.Drone();
            try
            {
                d = GetDrone(id);
                if (d.Status == BO.DroneStatus.Maintenance)
                {
                    dl.UpdateDischargeDrone(id);
                    BO.DroneToL droneToList = new BO.DroneToL();
                    droneToList = GetDroneToL(id);
                    dList.Remove(droneToList);
                    droneToList.Status = BO.DroneStatus.Avaliable;
                    droneToList.Battery += ;//////////////////
                    dList.Add(droneToList);
                }
                else
                    throw new BO.NotMaintenanceDroneException(id);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }

        }
        /// <summary>
        /// A function that returns the drone
        /// </summary>
        /// <param name="id">The id of a drone to get</param>
        /// <returns>The drone</returns>
        public BO.Drone GetDrone(int id)
        {
            BO.Drone drone = new BO.Drone();
            IDAL.DO.Drone d = new IDAL.DO.Drone();
            try
            {
                d = dl.GetDrone(id);
                drone.Id = d.Id;
                drone.Model = d.Model;
                drone.Weight = (BO.WeightCategories)d.MaxWeight;
                drone.Battery = GetDroneToL(id).Battery;
                drone.Status = GetDroneToL(id).Status;
                if (drone.Status == BO.DroneStatus.Delivery)
                {
                    drone.MyParcel = new BO.ParcelInTran();
                    drone.MyParcel.Id = GetDroneToL(id).ParcelId;
                    if (getParcelStatus(id) == BO.ParcelStatus.Connected)
                        drone.MyParcel.ParcelStatus = BO.ParcelInTranStatus.WaitToCollect;
                    if (getParcelStatus(id) == BO.ParcelStatus.PickedUp)
                        drone.MyParcel.ParcelStatus = BO.ParcelInTranStatus.OnWay;
                    BO.Parcel p = GetParcel(GetDroneToL(id).ParcelId);
                    drone.MyParcel.Weight = p.Weight;
                    drone.MyParcel.Priority = p.Priority;
                    drone.MyParcel.Sender = new BO.CustomerInP() { Id = p.Sender.Id, Name = p.Sender.Name };
                    drone.MyParcel.Receiver = new BO.CustomerInP() { Id = p.Receiver.Id, Name = p.Receiver.Name };
                    drone.MyParcel.Collection = new BO.Location() { Longitude = GetCustomer(p.Sender.Id).Place.Longitude, Latitude = GetCustomer(p.Sender.Id).Place.Latitude };
                    drone.MyParcel.Destination = new BO.Location() { Longitude = GetCustomer(p.Receiver.Id).Place.Longitude, Latitude = GetCustomer(p.Receiver.Id).Place.Latitude };
                    drone.MyParcel.Distance =;///////////////////
                }
                drone.CurrentPlace = new BO.Location();
                drone.CurrentPlace = GetDroneToL(id).CurrentPlace;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return drone;
        }
        /// <summary>
        /// A function that returns the drones
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<BO.Drone> DroneList()
        {
            return from item in dl.ListDrone()
                   select GetDrone(item.Id);
        }
    }
}
