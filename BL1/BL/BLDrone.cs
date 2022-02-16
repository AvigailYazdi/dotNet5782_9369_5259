using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using DalApi;

namespace BL
{
    sealed partial class BL: IBL
    {
        private object drone;
        /// <summary>
        /// A function that adds a drone to the data base
        /// </summary>
        /// <param name="droneBo"> The drone to add</param>
        public void AddDrone(BO.Drone droneBo ,int stationId)
        {
            DO.Drone droneDo = new DO.Drone()
            {
                Id = droneBo.Id,
                Model = droneBo.Model,
                MaxWeight = (DO.WeightCategories)droneBo.Weight
            };
            try
            {
                dl.AddDrone(droneDo);
                BO.BaseStation stationBo = GetStation(stationId);
                BO.DroneToL droneToList = new BO.DroneToL()
                {
                    Id = droneBo.Id,
                    Model = droneBo.Model,
                    Weight = droneBo.Weight,
                    Battery = rand.Next(20 * 100, 40 * 100) / 100.0,
                    Status = BO.DroneStatus.Maintenance,
                    CurrentPlace = new BO.Location() { Longitude = stationBo.Place.Longitude, Latitude = stationBo.Place.Latitude },
                    ParcelId = -1
                };
                dList.Add(droneToList);
                //if (stationBo.Id == 0)
                //    throw new BO.NotAvaliableStationException();
                dl.UpdateChargeDrone(droneBo.Id, stationBo.Id);
            }
            catch (DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
            catch (DO.MissingIdException ex)
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
                DO.Drone droneDo = dl.GetDrone(id);
                droneDo.Id = id;
                droneDo.Model = name;
                dl.UpdateDrone(droneDo);
            }
            catch (DO.MissingIdException ex)
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
                    DO.BaseStation b = closeStation(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude);
                    double distance = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, b.Longitude, b.Latitude);
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
            catch (DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that discharges a drone
        /// </summary>
        /// <param name="id">the id of a drone to discharge</param>
        /// <param name="time">the time that the drone was in charging</param>
        public void UpdateDisChargeDrone(int id)
        {
            try
            {
                BO.DroneToL droneToList = GetDroneToL(id);
                if (droneToList.Status == BO.DroneStatus.Maintenance)
                {
                    TimeSpan? time = DateTime.Now - dl.GetDroneCharge(droneToList.Id).insertTime;
                    dl.UpdateDischargeDrone(id);//dal
                    droneToList.Status = BO.DroneStatus.Avaliable;
                    droneToList.Battery =(int)( Math.Min(time.Value.TotalHours * electricUse[4] + droneToList.Battery, 100)*100)/100.0;
                    UpdateDroneToL(droneToList);
                }
                else
                    throw new BO.NotMaintenanceDroneException(id);
            }
            catch (DO.MissingIdException ex)
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
                    if (GetParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.Connected)
                        droneBo.MyParcel.ParcelStatus = BO.ParcelInTranStatus.WaitToCollect;
                    else if (GetParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.PickedUp)
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
            catch (DO.MissingIdException ex)
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

        public void NextState(int id)
        {
            bool flag = false;
            BO.Drone d = GetDrone(id);
            switch (d.Status)
            {
                case BO.DroneStatus.Avaliable:
                    try
                    {
                        UpdateParcelToDrone(id);
                    }
                    catch(Exception)
                    {
                        UpdateDroneToCharge(id);
                    }
                    break;
                case BO.DroneStatus.Maintenance:
                    if(d.Battery==100)
                    {
                        UpdateDisChargeDrone(id);
                        UpdateParcelToDrone(id);
                    }
                    else
                    {
                        flag = true;
                        //d.Battery= (int)(Math.Min(5 + d.Battery, 100) * 100) / 100.0;
                    }
                    break;
                case BO.DroneStatus.Delivery:
                    switch (d.MyParcel.ParcelStatus)
                    {
                        case BO.ParcelInTranStatus.WaitToCollect:
                            UpdateParcelCollect(id);
                            break;
                        case BO.ParcelInTranStatus.OnWay:
                            UpdateParcelProvide(id);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            d = GetDrone(id);
            if(flag)
                d.Battery = (int)(Math.Min(5 + d.Battery, 100) * 100) / 100.0;
            BO.DroneToL dlist = new BO.DroneToL()
            {
                Id = d.Id,
                Model = d.Model,
                Weight = d.Weight,
                Battery = d.Battery,
                Status = d.Status,
                CurrentPlace = new BO.Location { Longitude = d.CurrentPlace.Longitude, Latitude = d.CurrentPlace.Latitude },
                ParcelId=(d.MyParcel==null)?-1:d.MyParcel.Id
            };
            UpdateDroneToL(dlist);
        }
    }
}
