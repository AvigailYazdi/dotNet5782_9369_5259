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
            droneToList.CurrentPlace = new BO.Location() { Longitude= drone.CurrentPlace.Longitude, Latitude= drone.CurrentPlace.Latitude};
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
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        public void UpdateDisChargeDrone(int id, double time)
        {

        }
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
                    if (GetParcelStatus(id) == BO.ParcelStatus.Connected)
                        drone.MyParcel.ParcelStatus = BO.ParcelInTranStatus.WaitToCollect;
                    if (GetParcelStatus(id) == BO.ParcelStatus.PickedUp)
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
        public IEnumerable<BO.Drone> DroneList()
        {
            return from item in dl.ListDrone()
                   select GetDrone(item.Id);
        }
    }
}
