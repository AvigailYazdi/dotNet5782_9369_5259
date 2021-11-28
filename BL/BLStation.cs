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
        public void UpdateStation(int id, string name, int numSlots)
        {
            try
            {
                IDAL.DO.BaseStation bs = dl.GetBaseStation(id);
                if (name != "")
                    bs.Name = name;
                if (numSlots != 0)
                {
                    int num = dl.numOfNotAvaliableSlots(id);
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
                                     let dc = dl.GetDronesCharge(item.DroneId)
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
        public IEnumerable<BO.BaseStation> StationList()
        {
            return from item in dl.ListBaseStation()
                   select GetStation(item.Id);
        }
        public IEnumerable<BO.BaseStation> AvaliableStationList()
        {
            return from item in dl.ListAvaliableSlots()
                   select GetStation(item.Id);
        }
    }
}
