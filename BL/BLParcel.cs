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
            IDAL.DO.Parcel p = dl.GetParcel(id);
            if (p.Delivered != t) // The parcel delivered
                return BO.ParcelStatus.Provided;
            if (p.PickedUp != t) // The parcel Picked up
                return BO.ParcelStatus.PickedUp;
            if (p.Scheduled != t) // The parcel connected
                return BO.ParcelStatus.Connected;
            return BO.ParcelStatus.Created; // The parcel created
        }
        /// <summary>
        /// A function that returns all the parcel the customer has sent
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels</returns>
        private IEnumerable<BO.ParcelAtC> getSendParcel(int id)
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
        /// <summary>
        /// A function that returns all the parcels the customer has recieved
        /// </summary>
        /// <param name="id"> The id of the customer</param>
        /// <returns> The list of the parcels </returns>
        private IEnumerable<BO.ParcelAtC> getRecievedParcel(int id)
        {
            return from item in dl.GetParcelsByPerdicate(item => item.TargetId == id)
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
        /// <summary>
        /// A function that adds a parcel to the data base
        /// </summary>
        /// <param name="parcel"> The parcel to add</param>
        public void AddParcel(BO.Parcel parcel)
        {
            IDAL.DO.Parcel p= new IDAL.DO.Parcel();
            p.SenderId = parcel.Sender.Id;
            p.TargetId = parcel.Receiver.Id;
            p.Weight = (IDAL.DO.WeightCategories)parcel.Weight;
            p.Priority = (IDAL.DO.Priorities)parcel.Priority;
            p.Requested = DateTime.Now;
            p.Scheduled = new DateTime();
            p.PickedUp = new DateTime(); 
            p.Delivered = new DateTime();
            try
            {
                dl.AddParcel(p);
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
            BO.DroneToL droneToList = new BO.DroneToL();
            droneToList = GetDroneToL(id);
            if (droneToList.Status == BO.DroneStatus.Avaliable)
            {
                IEnumerable<IDAL.DO.Parcel> p = dl.ListParcel();
                IDAL.DO.Parcel pTemp = p.ElementAtOrDefault(0);
                double dis1, dis2;
                foreach (var item in dl.ListParcel())
                {
                    if (item.Priority > pTemp.Priority)
                        pTemp = item;
                    else if (item.Priority == pTemp.Priority && item.Weight > pTemp.Weight && (BO.WeightCategories)item.Weight <= droneToList.Weight)
                        pTemp = item;
                    dis1 = dl.Distance(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude,1, item.SenderId);
                    dis2 = dl.Distance(droneToList.CurrentPlace.Longitude, droneToList.CurrentPlace.Latitude, 1, pTemp.SenderId);
                    if(item.Priority == pTemp.Priority && item.Weight == pTemp.Weight && dis1<dis2)
                        pTemp = item;
                }
                dList.Remove(droneToList);
                droneToList.Status = BO.DroneStatus.Delivery;
                droneToList.ParcelId = pTemp.Id;
                dList.Add(droneToList);
                dl.DeleteParcel(pTemp);
                pTemp.DroneId = droneToList.Id;
                pTemp.Scheduled = DateTime.Now;;
                dl.AddParcel(pTemp);
            }
            else
                throw new BO.NotAvaliableDroneException(id);
        }
        /// <summary>
        /// A function that updates a collect time
        /// </summary>
        /// <param name="id">the id of a drone to collect</param>
        public void UpdateParcelCollect(int id)
        {
            BO.DroneToL d = new BO.DroneToL();
            DateTime t = new DateTime();
            d = GetDroneToL(id);
            if(d.Status==BO.DroneStatus.Delivery && GetParcel(d.ParcelId).PickedUp==t)
            {
                dList.Remove(d);
                d.Battery =;////////////
                BO.Customer c = new BO.Customer();
                IDAL.DO.Parcel p = dl.GetParcel(d.ParcelId);
                c = GetCustomer(p.SenderId);
                d.CurrentPlace = new BO.Location() { Longitude = c.Place.Longitude, Latitude = c.Place.Latitude };
                dList.Add(d);
                dl.DeleteParcel(p.Id);
                p.PickedUp = DateTime.Now;
                dl.AddParcel(p);
            }
            else
                throw new BO.NotInDeliveryException(id);

        }
        public void UpdateParcelProvide(int id)
        {

        }
        /// <summary>
        /// A function that returns a parcel
        /// </summary>
        /// <param name="id">The id of the parcel to get</param>
        /// <returns>The parcel</returns>
        public BO.Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel p= new IDAL.DO.Parcel();
            BO.Parcel parcel = new BO.Parcel();
            try
            {
                p = dl.GetParcel(id);
                parcel.Id = p.Id;
                parcel.Sender = new BO.CustomerInP()
                {
                    Id = p.SenderId,
                    Name = getCustomerName(p.SenderId)
                };
                parcel.Receiver = new BO.CustomerInP()
                {
                    Id = p.TargetId,
                    Name = getCustomerName(p.TargetId)
                };
                parcel.Weight = (BO.WeightCategories)p.Weight;
                parcel.Priority = (BO.Priorities)p.Priority;
                if (getParcelStatus(p.Id) != BO.ParcelStatus.Created)
                {
                    parcel.MyDrone = new BO.DroneInP()
                    {
                        Id = p.DroneId,
                        Battery = GetDroneToL(p.DroneId).Battery,
                        CurrentPlace = new BO.Location()
                        {
                            Longitude = GetDroneToL(p.DroneId).CurrentPlace.Longitude,
                            Latitude = GetDroneToL(p.DroneId).CurrentPlace.Latitude
                        }
                    };
                }
                parcel.Requested = p.Requested;
                parcel.Scheduled = p.Scheduled;
                parcel.PickedUp = p.PickedUp;
                parcel.Delivered = p.Delivered;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return parcel;
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
