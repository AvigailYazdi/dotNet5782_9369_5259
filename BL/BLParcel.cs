using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
namespace IBL
{
    public partial class BL : IBL
    {
        private object parcel;
        /// <summary>
        /// A function that returns the status of the parcel
        /// </summary>
        /// <param name="id"> The parcel</param>
        /// <returns> Returns the status</returns>
        private BO.ParcelStatus getParcelStatus(int id)
        {
            DateTime t = new DateTime();
            try
            {
                IDAL.DO.Parcel p = dl.GetParcel(id);
                if (p.Delivered != t) // The parcel delivered
                    return BO.ParcelStatus.Provided;
                if (p.PickedUp != t) // The parcel Picked up
                    return BO.ParcelStatus.PickedUp;
                if (p.Scheduled != t) // The parcel connected
                    return BO.ParcelStatus.Connected;
                return BO.ParcelStatus.Created; // The parcel created
            }
            catch(IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that returns all the parcel the customer has sent
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels</returns>
        private IEnumerable<BO.ParcelAtC> getSendParcel(int id)
        {
            try
            {
                return from item in dl.GetParcelsByPerdicate(item => item.SenderId == id)
                       let p = dl.GetParcel(item.Id)
                       select new BO.ParcelAtC()
                       {
                           Id = p.Id,
                           Weight = (BO.WeightCategories)p.Weight,
                           Priority = (BO.Priorities)p.Priority,
                           Status = getParcelStatus(p.Id),
                           OtherC = new BO.CustomerInP()
                           {
                               Id = p.Id,
                               Name = getCustomerName(p.SenderId)
                           }
                       };
            }
            catch(IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName); 
            }
        }
        /// <summary>
        /// A function that returns all the parcels the customer has recieved
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels </returns>
        private IEnumerable<BO.ParcelAtC> getRecievedParcel(int id)
        {
            try
            {
                return from item in dl.GetParcelsByPerdicate(item => item.TargetId == id&& getParcelStatus(item.Id)==BO.ParcelStatus.Provided)
                       let p = dl.GetParcel(item.Id)
                       select new BO.ParcelAtC()
                       {
                           Id = p.Id,
                           Weight = (BO.WeightCategories)p.Weight,
                           Priority = (BO.Priorities)p.Priority,
                           Status = getParcelStatus(p.Id),
                           OtherC = new BO.CustomerInP()
                           {
                               Id = p.Id,
                               Name = getCustomerName(p.TargetId)
                           }
                       };
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }

        }
        /// <summary>
        /// A function that adds a parcel to the data base
        /// </summary>
        /// <param name="parcel"> The parcel to add</param>
        public void AddParcel(BO.Parcel parcel)
        {
            IDAL.DO.Parcel parcelDo = new IDAL.DO.Parcel()
            {
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Receiver.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                Requested = DateTime.Now,
                Scheduled = new DateTime(),
                PickedUp = new DateTime(),
                Delivered = new DateTime()
            };
            try
            {
                dl.AddParcel(parcelDo);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that connect a parcel to a drone
        /// </summary>
        /// <param name="id">the id of a drone</param>
        public void UpdateParcelToDrone(int id)
        {
            BO.DroneToL droneToList = GetDroneToL(id);
            try
            {
                if (droneToList.Status == BO.DroneStatus.Avaliable)
                {
                    IEnumerable<IDAL.DO.Parcel> pList=dl.GetParcelsByPerdicate(p => p.Weight <= (IDAL.DO.WeightCategories)droneToList.Weight && p.DroneId == 0);
                    IDAL.DO.Parcel pTemp = pList.ElementAtOrDefault(0);
                    IDAL.DO.Customer senderDo, targetDo;
                    double dis1, dis2, dis3;
                    foreach (var item in pList.ToList())
                    {
                        senderDo = dl.GetCustomer(item.SenderId);
                        dis1 = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, senderDo.Longitude, senderDo.Latitude);
                        targetDo = dl.GetCustomer(item.TargetId);
                        dis2 = dl.DistanceInKm(targetDo.Longitude, targetDo.Latitude, senderDo.Longitude, senderDo.Latitude);
                        IDAL.DO.BaseStation b = closeStation(targetDo.Longitude, targetDo.Latitude);
                        dis3 = shortDis(targetDo.Longitude, targetDo.Latitude, b);
                        if (getBattery(dis1 + dis2 + dis3, id) <= droneToList.Battery)
                        {
                            if (item.Priority > pTemp.Priority)
                                pTemp = item;
                            else if (item.Priority == pTemp.Priority && item.Weight > pTemp.Weight)
                                pTemp = item;
                            else if(item.Priority == pTemp.Priority && item.Weight == pTemp.Weight)
                            {
                                dis1 = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, senderDo.Longitude, senderDo.Latitude);
                                senderDo = dl.GetCustomer(pTemp.SenderId);
                                dis2 = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, senderDo.Longitude, senderDo.Latitude);
                                if (dis1 < dis2)
                                    pTemp = item;
                            }
                        }
                    }
                    if (pTemp.Id != 0)
                    {
                        droneToList.Status = BO.DroneStatus.Delivery;
                        droneToList.ParcelId = pTemp.Id;
                        UpdateDroneToL(droneToList);
                        dl.UpdateParcelToDrone(id, pTemp.Id);
                    }
                }
                else
                    throw new BO.NotAvaliableDroneException(id);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
            catch(IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that updates a collect time
        /// </summary>
        /// <param name="id">the id of a drone to collect</param>
        public void UpdateParcelCollect(int id)
        {
            BO.DroneToL droneToList = GetDroneToL(id);
            double dis;
            try
            {
                if (droneToList.Status == BO.DroneStatus.Delivery && getParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.Connected)
                {
                    IDAL.DO.Parcel p = dl.GetParcel(droneToList.ParcelId);
                    IDAL.DO.Customer c = dl.GetCustomer(p.SenderId);
                    dis = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, c.Longitude, c.Latitude);
                    droneToList.CurrentPlace = new BO.Location() { Longitude = c.Longitude, Latitude = c.Latitude };
                    droneToList.Battery = Math.Max(0, droneToList.Battery - getBattery(dis, id)); ;
                    UpdateDroneToL(droneToList);
                    dl.UpdateParcelCollect(droneToList.ParcelId);
                }
                else
                    throw new BO.NotInDeliveryException(id);///////////////////////זריקה
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
        }
        public void UpdateParcelProvide(int id)
        {
            BO.DroneToL droneToList = GetDroneToL(id);
            double dis;
            try
            {
                if (droneToList.Status == BO.DroneStatus.Delivery && getParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.PickedUp)
                {
                    IDAL.DO.Parcel p = dl.GetParcel(droneToList.ParcelId);
                    IDAL.DO.Customer c = dl.GetCustomer(p.TargetId);
                    dis = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, c.Longitude, c.Latitude);
                    droneToList.CurrentPlace = new BO.Location() { Longitude = c.Longitude, Latitude = c.Latitude };
                    droneToList.Battery =  Math.Max(0, droneToList.Battery - getBattery(dis, id)); ;
                    droneToList.Status = BO.DroneStatus.Avaliable;
                    UpdateDroneToL(droneToList);
                    dl.UpdateParcelDelivery(droneToList.ParcelId);
                }
                else
                    throw new BO.NotInDeliveryException(id);///////////////////////
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that returns a parcel
        /// </summary>
        /// <param name="id">The id of the parcel to get</param>
        /// <returns>The parcel</returns>
        public BO.Parcel GetParcel(int id)
        {
            BO.Parcel parcelBo = new BO.Parcel() { };
            try
            {
                IDAL.DO.Parcel parcelDo = dl.GetParcel(id);
                parcelBo.Id = parcelDo.Id;
                parcelBo.Sender = new BO.CustomerInP()
                {
                    Id = parcelDo.SenderId,
                    Name = getCustomerName(parcelDo.SenderId)
                };
                parcelBo.Receiver = new BO.CustomerInP()
                {
                    Id = parcelDo.TargetId,
                    Name = getCustomerName(parcelDo.TargetId)
                };
                parcelBo.Weight = (BO.WeightCategories)parcelDo.Weight;
                parcelBo.Priority = (BO.Priorities)parcelDo.Priority;
                if (parcelDo.DroneId != 0)
                {
                    parcelBo.MyDrone = new BO.DroneInP()
                    {
                        Id = parcelDo.DroneId,
                        Battery = GetDroneToL(parcelDo.DroneId).Battery,
                        CurrentPlace = GetDroneToL(parcelDo.DroneId).CurrentPlace
                    };
                }
                parcelBo.Requested = parcelDo.Requested;
                parcelBo.Scheduled = parcelDo.Scheduled;
                parcelBo.PickedUp = parcelDo.PickedUp;
                parcelBo.Delivered = parcelDo.Delivered;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return parcelBo;
        }
        /// <summary>
        /// A function that returns the parcels
        /// </summary>
        /// <returns>List of parcels</returns>
        public IEnumerable<BO.Parcel> ParcelList()
        {
            return from item in dl.ListParcel()
                   select GetParcel(item.Id);
        }
        /// <summary>
        /// A function that returns the parcels that are not connected to a drone
        /// </summary>
        /// <returns> not connected parcels list</returns>
        public IEnumerable<BO.Parcel> NotConnectedParcelList()
        {
            return from item in dl.ListNotConnected()
                   select GetParcel(item.Id);
        }
    }
}
