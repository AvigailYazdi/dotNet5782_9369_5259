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
        private object station;
        /// <summary>
        /// A function that adds a station to the list
        /// </summary>
        /// <param name="stationBo">the station to add</param>
        public void AddStation(BO.BaseStation stationBo)
        {
            DO.BaseStation stationDo = new DO.BaseStation()
            {
                Id = stationBo.Id,
                Name = stationBo.Name,
                Longitude = stationBo.Place.Longitude,
                Latitude = stationBo.Place.Latitude,
                ChargeSlots = stationBo.AvaliableSlots
            };
            try
            {
                dl.AddBaseStation(stationDo);
            }
            catch (DO.DuplicateIdException ex)
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
                DO.BaseStation stationDo = dl.GetBaseStation(id);
                if (name != "")
                    stationDo.Name = name;
                int num = dl.NumOfNotAvaliableSlots(id);
                if (numSlots- num > 0)
                    stationDo.ChargeSlots = numSlots - num;
                else
                    throw new BO.NegativeSlotsException(id);
                dl.UpdateStation(stationDo);
            }
            catch (DO.MissingIdException ex)
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
            BO.BaseStation stationBo= new BO.BaseStation();
            try
            {
                DO.BaseStation stationDo = dl.GetBaseStation(id);
                stationBo.Id = stationDo.Id;
                stationBo.Name = stationDo.Name;
                stationBo.Place = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                stationBo.AvaliableSlots = stationDo.ChargeSlots;
                stationBo.DroneSlots = from item in dl.GetDronesInChargeByPerdicate(item => item.StationId == id)
                                     let dc = dl.GetDroneCharge(item.DroneId)
                                     select new BO.DroneInCharge()
                                     {
                                         Id = dc.DroneId,
                                         Battery = GetDroneToL(dc.DroneId).Battery
                                     };
            }
            catch (DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }
            return stationBo;
        }
        /// <summary>
        /// A function that returns stations
        /// </summary>
        /// <returns>List of stations</returns>
        public IEnumerable<BO.StationToL> StationList()
        {
            return from item in dl.ListBaseStation()
                   let s = GetStation(item.Id)
                   select new BO.StationToL()
                   {
                       Id= s.Id,
                       Name= s.Name,
                       AvaliableSlots= s.AvaliableSlots,
                       DisAvaliableSlots= dl.NumOfNotAvaliableSlots(s.Id)
                   };
        }
        /// <summary>
        /// A function that returns the stations with avaliable slots
        /// </summary>
        /// <returns>List of stations with avaliable slots</returns>
        public IEnumerable<BO.BaseStation> AvaliableStationList()
        {
            return from item in dl.GetStationsByPerdicate(s => s.ChargeSlots != 0)
                   select GetStation(item.Id);
        }
    }
}
