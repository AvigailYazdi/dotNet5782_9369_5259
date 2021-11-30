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
        private double [] electricUse;
        /// <summary>
        /// A constructor
        /// </summary>
        public BL()
        {
            rand = new Random(DateTime.Now.Millisecond);
            dl = new DalObject.DalObject();//intilaize dl
            electricUse = dl.ElectricUse();//electric use
            dList = (from item in dl.ListDrone()//dList
                     select new BO.DroneToL()
                     {
                         Id = item.Id,
                         Model = item.Model,
                         Weight = (BO.WeightCategories)item.MaxWeight,
                         Battery = 0,
                         Status = BO.DroneStatus.Avaliable,
                         CurrentPlace = new BO.Location(),
                         ParcelId = dl.GetConnectParcel(item.Id)
                     }).ToList();
            DateTime t = new DateTime();
            IDAL.DO.BaseStation stationDo;
            double distance1, distance2;
            try
            {
                foreach (var item in dList.ToList())
                {
                    BO.DroneToL temp = GetDroneToL(item.Id);
                    if (temp.ParcelId > 0 && dl.GetParcel(temp.ParcelId).Delivered == t)
                    {
                        IDAL.DO.Parcel p = dl.GetParcel(temp.ParcelId);
                        temp.Status = BO.DroneStatus.Delivery;//
                        IDAL.DO.Customer senderDo = dl.GetCustomer(p.SenderId);
                        IDAL.DO.Customer targetDo = dl.GetCustomer(p.TargetId);
                        if (p.PickedUp == t)
                        {
                            stationDo = closeStation(senderDo.Longitude, senderDo.Latitude);
                            temp.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                        }
                        else
                            temp.CurrentPlace = new BO.Location() { Longitude = senderDo.Longitude, Latitude = senderDo.Latitude };
                        distance1 = dl.DistanceInKm(temp.CurrentPlace.Longitude, temp.CurrentPlace.Latitude, targetDo.Longitude, targetDo.Latitude);
                        stationDo = closeStation(targetDo.Longitude, targetDo.Latitude);
                        distance2 = shortDis(targetDo.Longitude, targetDo.Latitude, stationDo);
                        temp.Battery = rand.Next((int)(getBattery(distance1 + distance2, temp.Id) * 100), 100 * 100) / 100.0;
                    }
                    if (temp.ParcelId <= 0)
                    {
                        temp.Status = (BO.DroneStatus)rand.Next(0, 2);
                    }
                    if (temp.Status == BO.DroneStatus.Maintenance)
                    {
                        IEnumerable<IDAL.DO.BaseStation> bs = dl.ListBaseStation();
                        stationDo = bs.ElementAtOrDefault(rand.Next(0, bs.Count()));
                        temp.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                        temp.Battery = rand.Next(0, 20 * 100) / 100.0;
                        dl.UpdateChargeDrone(temp.Id, stationDo.Id);
                    }
                    if (temp.Status == BO.DroneStatus.Avaliable)
                    {
                        IEnumerable<IDAL.DO.Parcel> p = dl.GetParcelsByPerdicate(it => it.Delivered != t);
                        if (p.Count() != 0)
                        {
                            IDAL.DO.Parcel pc = p.ElementAtOrDefault(rand.Next(0, p.Count()));
                            temp.CurrentPlace = new BO.Location() { Longitude = dl.GetCustomer(pc.TargetId).Longitude, Latitude = dl.GetCustomer(pc.TargetId).Latitude };
                            stationDo = closeStation(temp.CurrentPlace.Longitude, temp.CurrentPlace.Latitude);
                            distance1 = shortDis(temp.CurrentPlace.Longitude, temp.CurrentPlace.Latitude, stationDo);
                            temp.Battery = rand.Next((int)(getBattery(distance1, stationDo.Id) * 100), 100 * 100) / 100.0;
                            dl.UpdateChargeDrone(temp.Id, stationDo.Id);
                        }
                    }
                    UpdateDroneToL(temp);
                }
            }
            catch(IDAL.DO.MissingIdException ex)

            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }

        }
        /// <summary>
        /// A function that returns the station is the closet to the location
        /// </summary>
        /// <param name="lon1">the longitude</param>
        /// <param name="lat1">the latitude</param>
        /// <returns>the closet station</returns>
        private IDAL.DO.BaseStation closeStation(double lon1, double lat1)
        {
            double min = 1000000000000;
            double dis;
            int id = 0;
            foreach (var item in dl.ListAvaliableSlots())
            {
                dis = dl.DistanceInKm(lon1, lat1, item.Longitude, item.Latitude);
                if (dis < min)
                {
                    min = dis;
                    id = item.Id;
                }
            }
            try
            {
                return dl.GetBaseStation(id);
            }
            catch(IDAL.DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }

        }
        /// <summary>
        /// the distance between the station and a location
        /// </summary>
        /// <param name="lon">logitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="b">the station</param>
        /// <returns>the distance</returns>
        private double shortDis(double lon, double lat, IDAL.DO.BaseStation b)
        {
            return dl.DistanceInKm(lon, lat, b.Longitude,b.Latitude);
        }
        /// <summary>
        /// A function that colculates the wasted batttery
        /// </summary>
        /// <param name="distance">distance</param>
        /// <param name="droneId">the drone </param>
        /// <returns></returns>
        private double getBattery(double distance,int droneId)
        {
            BO.DroneToL droneList = GetDroneToL(droneId);
            double battery=-1;
            if (droneList.Status == BO.DroneStatus.Avaliable)
                battery= electricUse[0] * distance;
            else if (droneList.Status == BO.DroneStatus.Delivery)
            {
                switch (GetParcel(droneList.ParcelId).Weight)
                {
                    case BO.WeightCategories.Light:
                        battery= electricUse[1] * distance;
                        break;
                    case BO.WeightCategories.Medium:
                        battery = electricUse[2] * distance;
                        break;
                    case BO.WeightCategories.Heavy:
                        battery = electricUse[3] * distance;
                        break;
                    default:
                        break;
                }
            }
            return ((int)battery*100)/100;
        }
    }
}
