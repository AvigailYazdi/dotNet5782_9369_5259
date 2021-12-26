using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;
using DO;
using DalApi;

namespace Dal
{
    sealed partial class DalObject : IDal
    {
        //private object parcel;
        /// <summary>
        /// A function that checks if a parcel appears in the list
        /// </summary>
        /// <param name="id">The id of the parcel</param>
        private bool checkP(int id)
        {
            return parcels.Any(ps => ps.Id == id);
        }

        /// <summary>
        /// A function that adds a parcel to the array
        /// </summary>
        /// <param name="p"> the parcel to add</param>
        public void AddParcel(Parcel p)
        {
            if (checkP(p.Id))
                throw new DuplicateIdException(p.Id, "Parcel");
            p.Id = config.parcelId++;
            parcels.Add(p);
        }

        /// <summary>
        /// A function that updates a parcel
        /// </summary>
        /// <param name="p">The updated parcel</param>
        public void UpdateParcel(Parcel p)
        {
            DeleteParcel(p.Id);
            AddParcel(p);
        }

        /// <summary>
        /// A function that connects between a parcel and a drone
        /// </summary>
        /// <param name="droneId"> the id of the requested drone</param>
        /// <param name="parcelId">the id of the requested parcel</param>
        public void UpdateParcelToDrone(int droneId, int parcelId)
        {
            if (!checkP(parcelId))
                throw new MissingIdException(parcelId, "Parcel");
            if (!checkD(droneId))
                throw new MissingIdException(droneId, "Drone");
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
            if (!checkP(parcelId))
                throw new MissingIdException(parcelId, "Parcel");
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
            if (!checkP(parcelId))
                throw new MissingIdException(parcelId, "Parcel");
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
            if (!checkP(id))
                throw new MissingIdException(id, "Parcel");
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
        /// A function that deletes a parcel from the list
        /// </summary>
        /// <param name="id"> the id of a parcel to delete</param>
        public void DeleteParcel(int id)
        {
            if (!checkP(id))
                throw new MissingIdException(id, "Parcel");
            parcels.RemoveAll(pr=>pr.Id==id);/////////////לשנות
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
