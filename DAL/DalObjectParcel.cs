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
        private object parcel;
        /// <summary>
        /// A function that checks if a parcel appears in the list
        /// </summary>
        /// <param name="id">The id of the parcel</param>
        private void checkP(int id)
        {
            if (!parcels.Any(ps => ps.Id == id))
                throw new MissingIdException(id, "Parcel");
        }
        /// <summary>
        /// A function that adds a parcel to the array
        /// </summary>
        /// <param name="p"> the parcel to add</param>
        public void AddParcel(Parcel p)
        {
            if (parcels.Any(ps => ps.Id == p.Id))
                throw new DuplicateIdException(p.Id, "Parcel");
            p.Id = config.parcelId++;
            parcels.Add(p);
        }
        /// <summary>
        /// A function that connects between a parcel and a drone
        /// </summary>
        /// <param name="droneId"> the id of the requested drone</param>
        /// <param name="parcelId">the id of the requested parcel</param>
        public void UpdateParcelToDrone(int droneId, int parcelId)
        {
            checkP(parcelId);
            checkD(droneId);
            for (int i = 0; i < parcels.Count; i++)
            {
                if (parcels[i].Id == parcelId)
                {
                    Parcel p = parcels[i];
                    p.DroneId = droneId;
                    p.Scheduled = DateTime.Now;
                    parcels[i] = p;
                    break;
                }
            }
        }
        /// <summary>
        /// A function that updates the time of picking up the parcel
        /// </summary>
        /// <param name="parcelId"> the id of parcel that picked up</param>
        public void UpdateParcelCollect(int parcelId)
        {
            checkP(parcelId);
            for (int i = 0; i < parcels.Count; i++)
            {
                if (parcels[i].Id == parcelId)
                {
                    Parcel p = parcels[i];
                    p.PickedUp = DateTime.Now;
                    parcels[i] = p;
                    break;
                }
            }
        }
        /// <summary>
        /// A function that updates the time of parcel delivery
        /// </summary>
        /// <param name="parcelId"> the id of the parcel </param>
        public void UpdateParcelDelivery(int parcelId)
        {
            checkP(parcelId);
            for (int i = 0; i < parcels.Count; i++)
            {
                if (parcels[i].Id == parcelId)
                {
                    Parcel p = parcels[i];
                    p.Delivered = DateTime.Now;
                    parcels[i] = p;
                    break;
                }
            }
        }
        /// <summary>
        /// A function that shows the requested parcel
        /// </summary>
        /// <param name="id"> the id of the requested parcel</param>
        /// <returns> returns the requested parcel</returns>
        public Parcel GetParcel(int id)
        {
            checkP(id);
            return parcels.Find(p => p.Id == id);
        }
        /// <summary>
        /// A function that showes the list of the parcels
        /// </summary>
        /// <returns> returns the list of the parcels</returns>
        public IEnumerable<Parcel> ListParcel()
        {
            return from item in parcels
                   select item;
        }
        /// <summary>
        /// A function that showes the list of the not connected parcels
        /// </summary>
        /// <returns> returns the list of the not connected parcels</returns>
        public IEnumerable<Parcel> ListNotConnected()
        {
            return GetParcelsByPerdicate(p => p.Id == 0);
        }
        /// <summary>
        /// A function that deletes a parcel from the list
        /// </summary>
        /// <param name="id"> the id of a parcel to delete</param>
        public void DeleteParcel(int id)
        {
            checkP(id);
            parcels.Remove(GetParcel(id));
        }
        /// <summary>
        /// A function that returns the parcels that stand in a condition
        /// </summary>
        /// <param name="predicate"> The condition</param>
        /// <returns> The parcels that stand in a condition </returns>
        public IEnumerable<Parcel> GetParcelsByPerdicate(Predicate<Parcel> predicate)
        {
            return from item in parcels
                   where predicate(item)
                   select item;
        }
    }
}
