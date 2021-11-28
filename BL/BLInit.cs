using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public partial class BL : IBL
    {
        private DalObject.DalObject dl;
        private static Random rand;
        private List<BO.DroneToL> dList;
        /// <summary>
        /// A constructor
        /// </summary>
        public BL()
        {
            rand = new Random(DateTime.Now.Millisecond);
            dl = new DalObject.DalObject();
            dList = (from item in dl.ListDrone()
                     select new BO.DroneToL()
                     {
                         Id = item.Id,
                         Model = item.Model,
                         Weight = (BO.WeightCategories)item.MaxWeight,
                         Battery = 0,
                         Status = 0,
                         CurrentPlace = new BO.Location(),
                         ParcelId = -1
                     }).ToList(); 
            DateTime t = new DateTime();
            foreach (var item in dList)
            {
                BO.DroneToL drone = new BO.DroneToL() { Id = item.Id };
                if (item.ParcelId > 0 && dl.GetParcel(item.ParcelId).Delivered == t)
                {
                    IDAL.DO.Parcel p = dl.GetParcel(item.ParcelId);
                    item.Status = BO.DroneStatus.Delivery;
                    if (p.PickedUp == t)
                        item.CurrentPlace = new BO.Location() { Longitude =};///////////////////
                    else
                        item.CurrentPlace = new BO.Location() { Longitude = GetCustomer(p.SenderId).Place.Longitude, Latitude = GetCustomer(p.SenderId).Place.Latitude };
                    item.Battery = rand.NextDouble()+rand.Next(GetMinBattery(),100);//////
                }
                if (item.ParcelId <= 0)
                {
                    item.Status = (BO.DroneStatus)rand.Next(0, 3);
                }
                if(item.Status==BO.DroneStatus.Maintenance)
                {
                    IEnumerable<IDAL.DO.BaseStation> b = dl.ListBaseStation();
                    IDAL.DO.BaseStation bs= b.ElementAtOrDefault(rand.Next(0, b.Count()));
                    dl.UpdateChargeDrone(item.Id, bs.Id);
                    item.CurrentPlace = new BO.Location() { Longitude = bs.Longitude, Latitude = bs.Latitude };
                    item.Battery = rand.Next(0, 21);
                }
                if (item.Status == BO.DroneStatus.Avaliable)
                {
                    IEnumerable<IDAL.DO.Parcel> p = dl.GetParcelsByPerdicate(it=> it.Delivered!=t);
                    IDAL.DO.Parcel pc = p.ElementAtOrDefault(rand.Next(0, p.Count()));
                    item.CurrentPlace = new BO.Location() { Longitude = dl.GetCustomer(pc.TargetId).Longitude, Latitude= dl.GetCustomer(pc.TargetId).Latitude };
                    item.Battery = rand.NextDouble() + rand.Next(GetMinBattery(), 100);//////
                }
                dList.Remove(drone);
                dList.Add(item);
            }

        }
        private IDAL.DO.BaseStation shortDis(double longitude, double latitude)
        {
            double min = 1000000000000;
            double dis;
            int id=0;
            foreach (var item in dl.ListAvaliableSlots())
            {
                dis = dl.Distance(longitude,latitude,0, item.Id);
                if (dis < min)
                {
                    min = dis;
                    id = item.Id;
                }
            }
            return dl.GetBaseStation(id);
        }
    }
}
