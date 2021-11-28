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
        private object station;
        /// <summary>
        /// A function that adds a station to the list
        /// </summary>
        /// <param name="station">the station to add</param>
        public void AddStation(BO.BaseStation station)
        {
            IDAL.DO.BaseStation s= new IDAL.DO.BaseStation(); 
            s.Id = station.Id;
            s.Name = station.Name;
            s.Longitude = station.Place.Longitude;
            s.Latitude = station.Place.Latitude;
            s.ChargeSlots = station.AvaliableSlots;
            try
            {
                dl.AddBaseStation(s);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new BO.DuplicateIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that sets detailes about a station
        /// </summary>
        /// <param name="id">the id of the station to change</param>
        /// <param name="name">the name to change to</param>
        /// <param name="numSlots">the num odf sots to change to</param>
        public void UpdateStation(int id, string name, int numSlots)
        {
            try
            {
                IDAL.DO.BaseStation bs = dl.GetBaseStation(id);
                if (name != "")
                    bs.Name = name;
                if (numSlots != 0)
                {
                    int num = dl.NumOfNotAvaliableSlots(id);
                    bs.ChargeSlots = numSlots - num;
                    if (bs.ChargeSlots < 0)
                        throw new BO.NegativeSlotsException(id);
                }
                dl.DeleteStation(id);
                dl.AddBaseStation(bs);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
        }
        /// <summary>
        /// A function that returns a station
        /// </summary>
        /// <param name="id">the id of a station to get</param>
        /// <returns>a station</returns>
        public BO.BaseStation GetStation(int id)
        {
            IDAL.DO.BaseStation b = new IDAL.DO.BaseStation();
            BO.BaseStation station= new BO.BaseStation();
            try
            {
                b = dl.GetBaseStation(id);
                station.Id = b.Id;
                station.Name = b.Name;
                station.Place.Longitude = b.Longitude;
                station.Place.Latitude = b.Latitude;
                station.AvaliableSlots = b.ChargeSlots;
                station.DroneSlots = from item in dl.GetDronesInChargeByPerdicate(item => item.StationId == id)
                                     let dc = dl.GetDroneCharge(item.DroneId)
                                     select new BO.DroneInCharge()
                                     {
                                         Id = dc.DroneId,
                                         Battery = GetDroneToL(dc.DroneId).Battery
                                     };
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return station;
        }
        /// <summary>
        /// A function that returns stations
        /// </summary>
        /// <returns>List of stations</returns>
        public IEnumerable<BO.BaseStation> StationList()
        {
            return from item in dl.ListBaseStation()
                   select GetStation(item.Id);
        }
        /// <summary>
        /// A function that returns the stations with avaliable slots
        /// </summary>
        /// <returns>List of stations with avaliable slots</returns>
        public IEnumerable<BO.BaseStation> AvaliableStationList()
        {
            return from item in dl.ListAvaliableSlots()
                   select GetStation(item.Id);
        }
    }
}
