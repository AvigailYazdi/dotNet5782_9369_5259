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
        private object station;
        /// <summary>
        /// A function that checks if a station appears in the list
        /// </summary>
        /// <param name="id">The id of the station</param>
        private void checkS(int id)
        {
            if (!stations.Any(b => b.Id == id))
                throw new MissingIdException(id, "Base Station");
        }
        /// <summary>
        /// A function that returns the number of not avaliable slots in a certain station
        /// </summary>
        /// <param name="id">the id of the station</param>
        /// <returns>the number of not avaliable slots</returns>
        public int NumOfNotAvaliableSlots(int id)
        {
            checkS(id);
            int count = 0;
            foreach (var item in dronesCharge)
            {
                if (item.StationId == id)
                    count++;
            }
            return count;
        }
        /// <summary>
        /// A function that adds a station to the array
        /// </summary>
        /// <param name="bs"> the station to add</param>
        public void AddBaseStation(BaseStation bs)
        {
            if (stations.Any(s => s.Id == bs.Id))
                throw new DuplicateIdException(bs.Id, "Base station");
            stations.Add(bs);
        }
        /// <summary>
        /// A function that shows the requested station
        /// </summary>
        /// <param name="id"> the id of the requested station</param>
        /// <returns> returns the requested station</returns>
        public BaseStation GetBaseStation(int id)
        {
            checkS(id);
            return stations.Find(s => s.Id == id);
        }
        /// <summary>
        /// A function that shows the list of the stations
        /// </summary>
        /// <returns> returns the list of the stations</returns>
        public IEnumerable<BaseStation> ListBaseStation()
        {
            return from item in stations
                   select item;
        }
        /// <summary>
        /// A function that showes the list of the stations with avaliable slots
        /// </summary>
        /// <returns> returns the list of the stations with avaliable slots</returns>
        public IEnumerable<BaseStation> ListAvaliableSlots()
        {
            return GetStationsByPerdicate(s => s.ChargeSlots != 0);
        }
        /// <summary>
        /// A function that deletes a station from the list
        /// </summary>
        /// <param name="id"> The station id to delete</param>
        public void DeleteStation(int id)
        {
            checkS(id);
            stations.Remove(GetBaseStation(id));
        }
        /// <summary>
        /// A function that returns the stations that stand in a condition 
        /// </summary>
        /// <param name="predicate">The condition to check</param>
        /// <returns>A collection of the stations that stand in the condition</returns>
        public IEnumerable<BaseStation> GetStationsByPerdicate(Predicate<BaseStation> predicate)
        {
            return from item in stations
                   where predicate(item)
                   select item;
        }
    }
}
