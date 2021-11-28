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
        public void UpdateParcelToDrone(int id)
        {

        }
        public void UpdateParcelCollect(int id)
        {

        }
        public void UpdateParcelProvide(int id)
        {

        }
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
                    Name = GetCustomerName(p.SenderId)
                };
                parcel.Receiver = new BO.CustomerInP()
                {
                    Id = p.TargetId,
                    Name = GetCustomerName(p.TargetId)
                };
                parcel.Weight = (BO.WeightCategories)p.Weight;
                parcel.Priority = (BO.Priorities)p.Priority;
                if (GetParcelStatus(p.Id) != BO.ParcelStatus.Created)
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
        public IEnumerable<BO.Parcel> ParcelList()
        {
            return from item in dl.ListParcel()
                   select GetParcel(item.Id);
        }
        public IEnumerable<BO.Parcel> NotConnectedParcelList()
        {
            return from item in dl.ListNotConnected()
                   select GetParcel(item.Id);
        }
    }
}
