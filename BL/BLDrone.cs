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
        /// <param name="droneBo"> The drone to add</param>
        public void AddDrone(BO.Drone droneBo, int stationId)
        {
            IDAL.DO.Drone droneDo = new IDAL.DO.Drone()
            {
                Id = droneBo.Id,
                Model = droneBo.Model,
                MaxWeight = (IDAL.DO.WeightCategories)droneBo.Weight
            };
            try
            {
                dl.AddDrone(droneDo);
                BO.DroneToL droneToList = new BO.DroneToL()
                {
                    Id = droneBo.Id,
                    Model = droneBo.Model,
                    Weight = droneBo.Weight,
                    Battery = rand.Next(20 * 100, 40 * 100) / 100.0,
                    Status = BO.DroneStatus.Maintenance,
                    CurrentPlace = GetStation(stationId).Place,
                    ParcelId = -1
                };
                dList.Add(droneToList);
                if (GetStation(stationId).AvaliableSlots > 0)
                    dl.UpdateChargeDrone(droneBo.Id, stationId);
                else
                    throw new BO.NotEnoughSlotsException();
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);

            }
        }
        /// <summary>
        /// A function that sets the name of a drone
        /// </summary>
        /// <param name="id">the drone id to change</param>
        /// <param name="name">the name to change to</param>
        public void UpdateDroneName(int id, string name)
        {
            try
            {
                IDAL.DO.Drone droneDo = dl.GetDrone(id);
                droneDo.Id = id;
                droneDo.Model = name;
                dl.UpdateDrone(droneDo);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
            BO.DroneToL droneToList = dList.Find(dr => dr.Id == id);
            droneToList.Model = name;
            UpdateDroneToL(droneToList);
        }
        /// <summary>
        /// A function that put a drone in charge
        /// </summary>
        /// <param name="id">the id of the drone to charge</param>
        public void UpdateDroneToCharge(int id)
        {
            try
            {
                BO.DroneToL droneToList = GetDroneToL(id);
                if (droneToList.Status == BO.DroneStatus.Avaliable)
                {
                    IDAL.DO.BaseStation b = closeStation(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude);
                    double distance = shortDis(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, b);
                    if (getBattery(distance,id) > droneToList.Battery)
                        throw new BO.NotEnoughBatteryException(id);
                    dl.UpdateChargeDrone(id, b.Id);//dal
                    droneToList.Status = BO.DroneStatus.Maintenance;
                    droneToList.Battery = Math.Max(0, droneToList.Battery-getBattery(distance, id));
                    droneToList.CurrentPlace = new BO.Location() { Longitude = b.Longitude, Latitude = b.Latitude };
                    UpdateDroneToL(droneToList);
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
            try
            {
                BO.DroneToL droneToList = GetDroneToL(id);
                if (droneToList.Status == BO.DroneStatus.Maintenance)
                {
                    dl.UpdateDischargeDrone(id);//dal
                    droneToList.Status = BO.DroneStatus.Avaliable;
                    droneToList.Battery = Math.Min(time * electricUse[4]+ droneToList.Battery, 100);
                    UpdateDroneToL(droneToList);
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
            BO.Drone droneBo = new BO.Drone();
            try
            {
                BO.DroneToL droneToList = GetDroneToL(id);
                droneBo.Id = droneToList.Id;
                droneBo.Model = droneToList.Model;
                droneBo.Weight = (BO.WeightCategories)droneToList.Weight;
                droneBo.Battery = droneToList.Battery;
                droneBo.Status = droneToList.Status;
                if (droneBo.Status == BO.DroneStatus.Delivery)
                {
                    droneBo.MyParcel = new BO.ParcelInTran() { };
                    droneBo.MyParcel.Id = droneToList.ParcelId;
                    if (getParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.Connected)
                        droneBo.MyParcel.ParcelStatus = BO.ParcelInTranStatus.WaitToCollect;
                    else if (getParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.PickedUp)
                        droneBo.MyParcel.ParcelStatus = BO.ParcelInTranStatus.OnWay;
                    BO.Parcel p = GetParcel(droneToList.ParcelId);
                    droneBo.MyParcel.Weight = p.Weight;
                    droneBo.MyParcel.Priority = p.Priority;
                    droneBo.MyParcel.Sender = p.Sender;
                    droneBo.MyParcel.Receiver = p.Receiver;
                    droneBo.MyParcel.Collection = GetCustomer(p.Sender.Id).Place;
                    droneBo.MyParcel.Destination =GetCustomer(p.Receiver.Id).Place;
                    droneBo.MyParcel.Distance =dl.DistanceInKm(droneBo.MyParcel.Collection.Longitude, droneBo.MyParcel.Collection.Latitude, droneBo.MyParcel.Destination.Longitude, droneBo.MyParcel.Destination.Latitude);
                }
                droneBo.CurrentPlace = GetDroneToL(id).CurrentPlace;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return droneBo;
        }
        /// <summary>
        /// A function that returns the drones
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<BO.DroneToL> DroneList()
        {
            return from item in dList
                   select item;
        }

        public IEnumerable<BO.DroneToL> GetDronesByPerdicate(Predicate<BO.DroneToL> predicate)
        {
            return from item in dList
                   where predicate(item)
                   select item;
        }
    }
}
