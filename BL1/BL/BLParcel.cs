using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using DalApi;

namespace BL
{
    sealed partial class BL : IBL
    {
        //private object parcel;
        /// <summary>
        /// A function that returns the parcels that sent and provieded
        /// </summary>
        /// <param name="id">The id of the customer</param>
        /// <returns> The parcels that sent and provieded </returns>
        private IEnumerable<BO.ParcelAtC> getSentAndProviededParcels(int id)
        {
            return from item in getSendParcel(id)
                   where GetParcelStatus(item.Id) == BO.ParcelStatus.Provided
                   select item;

        }
        /// <summary>
        /// A function that returns the parcels that sent and not provieded
        /// </summary>
        /// <param name="id">The id of the customer</param>
        /// <returns> The parcels that sent and not provieded </returns>
        private IEnumerable<BO.ParcelAtC> getSentAndNotProviededParcels(int id)
        {
            return from item in getSendParcel(id)
                   where GetParcelStatus(item.Id) != BO.ParcelStatus.Provided
                   select item;
        }
        /// <summary>
        /// A function that returns all parcels that on way to a customer
        /// </summary>
        /// <param name="id"> Thhe id of tje customer </param>
        /// <returns> Parcels that on way</returns>
        private IEnumerable<BO.ParcelAtC> getOnWayParcels(int id)
        {
            return from item in dl.GetParcelsByPerdicate(item => item.TargetId == id && GetParcelStatus(item.Id) == BO.ParcelStatus.PickedUp)
                   let p = dl.GetParcel(item.Id)
                   select new BO.ParcelAtC()
                   {
                       Id = p.Id,
                       Weight = (BO.WeightCategories)p.Weight,
                       Priority = (BO.Priorities)p.Priority,
                       Status = GetParcelStatus(p.Id),
                       OtherC = new BO.CustomerInP()
                       {
                           Id = p.Id,
                           Name = getCustomerName(p.TargetId)
                       }
                   };
        }
        /// <summary>
        /// A function that returns the status of the parcel
        /// </summary>
        /// <param name="id"> The parcel</param>
        /// <returns> Returns the status</returns>
        public BO.ParcelStatus GetParcelStatus(int id)
        {
            try
            {
                DO.Parcel p = dl.GetParcel(id);
                if (p.Delivered != null) // The parcel delivered
                    return BO.ParcelStatus.Provided;
                if (p.PickedUp != null) // The parcel Picked up
                    return BO.ParcelStatus.PickedUp;
                if (p.Scheduled != null) // The parcel connected
                    return BO.ParcelStatus.Connected;
                return BO.ParcelStatus.Created; // The parcel created
            }
            catch(DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that returns all the parcel the customer has sent
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels</returns>
        public IEnumerable<BO.ParcelAtC> getSendParcel(int id)
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
                           Status = GetParcelStatus(p.Id),
                           OtherC = new BO.CustomerInP()
                           {
                               Id = p.Id,
                               Name = getCustomerName(p.TargetId)
                           }
                       };
            }
            catch(DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName); 
            }
        }
        /// <summary>
        /// A function that returns all the parcels the customer has recieved
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels </returns>
        public IEnumerable<BO.ParcelAtC> getRecievedParcel(int id)
        {
            try
            {
                return from item in dl.GetParcelsByPerdicate(item => item.TargetId == id && GetParcelStatus(item.Id)==BO.ParcelStatus.Provided)
                       let p = dl.GetParcel(item.Id)
                       select new BO.ParcelAtC()
                       {
                           Id = p.Id,
                           Weight = (BO.WeightCategories)p.Weight,
                           Priority = (BO.Priorities)p.Priority,
                           Status = GetParcelStatus(p.Id),
                           OtherC = new BO.CustomerInP()
                           {
                               Id = p.Id,
                               Name = getCustomerName(p.SenderId)
                           }
                       };
            }
            catch (DO.MissingIdException ex)
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
            DO.Parcel parcelDo = new DO.Parcel()
            {
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Receiver.Id,
                Weight = (DO.WeightCategories)parcel.Weight,
                Priority = (DO.Priorities)parcel.Priority,
                Requested = DateTime.Now,
                Scheduled = new DateTime(),
                PickedUp = new DateTime(),
                Delivered = new DateTime()
            };
            try
            {
                dl.AddParcel(parcelDo);
            }
            catch (DO.DuplicateIdException ex)
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
                    IEnumerable<DO.Parcel> pList=dl.GetParcelsByPerdicate(p => p.Weight <= (DO.WeightCategories)droneToList.Weight && p.DroneId == 0);
                    DO.Parcel pTemp = pList.First();
                    DO.Customer senderDo, targetDo;
                    double dis1, dis2, dis3;
                    bool flag = false;
                    foreach(var item in pList.ToList())
                    {
                        senderDo = dl.GetCustomer(item.SenderId);
                        dis1 = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, senderDo.Longitude, senderDo.Latitude);
                        targetDo = dl.GetCustomer(item.TargetId);
                        dis2 = dl.DistanceInKm(targetDo.Longitude, targetDo.Latitude, senderDo.Longitude, senderDo.Latitude);
                        DO.BaseStation b = closeStation(targetDo.Longitude, targetDo.Latitude);
                        dis3 = dl.DistanceInKm(targetDo.Longitude, targetDo.Latitude, b.Longitude, b.Latitude);
                        if (getBattery(dis1, id) + getWasteBattery(dis2 + dis3, item.Id) <= droneToList.Battery) 
                        {
                            if (item.Priority > pTemp.Priority)
                            {
                                flag = true;
                                pTemp = item;
                            }
                            else if (item.Priority == pTemp.Priority && item.Weight > pTemp.Weight)
                            {
                                flag = true;
                                pTemp = item;
                            }
                            else if (item.Priority == pTemp.Priority && item.Weight == pTemp.Weight)
                            {
                                dis1 = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, senderDo.Longitude, senderDo.Latitude);
                                senderDo = dl.GetCustomer(pTemp.SenderId);
                                dis2 = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, senderDo.Longitude, senderDo.Latitude);
                                if (dis1 <= dis2)
                                {
                                    pTemp = item;
                                    flag = true;
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        droneToList.Status = BO.DroneStatus.Delivery;
                        droneToList.ParcelId = pTemp.Id;
                        UpdateDroneToL(droneToList);
                        dl.UpdateParcelToDrone(id, pTemp.Id);
                    }
                    else
                        UpdateDroneToCharge(id);
                }
                else
                    throw new BO.NotAvaliableDroneException(id);
            }
            catch (DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
            catch(DO.MissingIdException ex)
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
                if (droneToList.Status == BO.DroneStatus.Delivery && GetParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.Connected)
                {
                    DO.Parcel p = dl.GetParcel(droneToList.ParcelId);
                    DO.Customer c = dl.GetCustomer(p.SenderId);
                    dis = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, c.Longitude, c.Latitude);
                    droneToList.CurrentPlace = new BO.Location() { Longitude = c.Longitude, Latitude = c.Latitude };
                    droneToList.Battery = ((int)((Math.Max(0, droneToList.Battery - getBattery(dis, id))) * 100)) / 100.0;
                    UpdateDroneToL(droneToList);
                    dl.UpdateParcelCollect(droneToList.ParcelId);
                }
                else
                    throw new BO.NotInDeliveryException(id);
            }
            catch (DO.DuplicateIdException ex)
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
                if (droneToList.Status == BO.DroneStatus.Delivery && GetParcelStatus(droneToList.ParcelId) == BO.ParcelStatus.PickedUp)
                {
                    DO.Parcel p = dl.GetParcel(droneToList.ParcelId);
                    DO.Customer c = dl.GetCustomer(p.TargetId);
                    dis = dl.DistanceInKm(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, c.Longitude, c.Latitude);
                    droneToList.CurrentPlace = new BO.Location() { Longitude = c.Longitude, Latitude = c.Latitude };
                    droneToList.Battery = ((int)((Math.Max(0, droneToList.Battery - getBattery(dis, id))) * 100)) / 100.0;
                    droneToList.Status = BO.DroneStatus.Avaliable;
                    UpdateDroneToL(droneToList);
                    dl.UpdateParcelDelivery(droneToList.ParcelId);
                    droneToList.ParcelId = -1;
                }
                else
                    throw new BO.NotInDeliveryException(id);
            }
            catch (DO.DuplicateIdException ex)
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
                DO.Parcel parcelDo = dl.GetParcel(id);
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
            catch (DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return parcelBo;
        }

        public BO.ParcelToL GetParcelToL(int id)
        {
            BO.Parcel p = GetParcel(id);
            return new BO.ParcelToL()
            {
                Id = p.Id,
                SenderName = p.Sender.Name,
                ReceiverName = p.Receiver.Name,
                Weight = p.Weight,
                Priority = p.Priority,
                Status = GetParcelStatus(p.Id)
            };
        }
        /// <summary>
        /// A function that returns the parcels
        /// </summary>
        /// <returns>List of parcels</returns>
        public IEnumerable<BO.ParcelToL> ParcelList()
        {
            return from item in dl.ListParcel()
                   let p = GetParcel(item.Id)
                   select new BO.ParcelToL()
                   {
                       Id=p.Id,
                       SenderName= p.Sender.Name,
                       ReceiverName= p.Receiver.Name,
                       Weight= p.Weight,
                       Priority= p.Priority,
                       Status= GetParcelStatus(p.Id)
                   };
        }
        /// <summary>
        /// A function that returns the parcels that are not connected to a drone
        /// </summary>
        /// <returns> not connected parcels list</returns>
        public IEnumerable<BO.Parcel> NotConnectedParcelList()
        {
            return from item in dl.GetParcelsByPerdicate(p => p.DroneId == 0)
                   select GetParcel(item.Id);
        }
        public IEnumerable<BO.ParcelToL> GetParcelByPredicate(Predicate<BO.ParcelToL> predicate)
        {
            return from item in ParcelList()
                   where predicate(item)
                   select item;
        }
        public void DeleteParcel(int id)
        {
            dl.DeleteParcel(id);
        }

        public int GetParcelId()
        {
            return dl.GetParcelId();
        }

    }
}
