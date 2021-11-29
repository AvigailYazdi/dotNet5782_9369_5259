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
        //שדה של מערך של הקונפיג
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
            foreach (var item in dList)
            {
                if (item.ParcelId > 0 && dl.GetParcel(item.ParcelId).Delivered == t)
                {
                    IDAL.DO.Parcel p = dl.GetParcel(item.ParcelId);
                    item.Status = BO.DroneStatus.Delivery;//
                    IDAL.DO.Customer senderDo= dl.GetCustomer(p.SenderId);
                    IDAL.DO.Customer targetDo = dl.GetCustomer(p.TargetId);
                    if (p.PickedUp == t)
                    {
                        stationDo = closeStation(senderDo.Longitude, senderDo.Latitude);
                        item.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                    }
                    else
                        item.CurrentPlace = new BO.Location() { Longitude = senderDo.Longitude, Latitude = senderDo.Latitude };
                    distance1 = dl.DistanceInKm(item.CurrentPlace.Longitude, item.CurrentPlace.Latitude, targetDo.Longitude,targetDo.Latitude); 
                    stationDo=closeStation(targetDo.Longitude, targetDo.Latitude);
                    distance2 = shortDis(targetDo.Longitude, targetDo.Latitude, stationDo);
                    item.Battery = rand.NextDouble()+rand.Next((int)getMinBattery(distance1+distance2,item.Id)+1,100);
                }
                if (item.ParcelId <= 0)
                {
                    item.Status = (BO.DroneStatus)rand.Next(0, 2);
                }
                if(item.Status==BO.DroneStatus.Maintenance)
                {
                    IEnumerable<IDAL.DO.BaseStation> bs = dl.ListBaseStation();
                    stationDo= bs.ElementAtOrDefault(rand.Next(0, bs.Count()));
                    item.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                    item.Battery = rand.NextDouble()+rand.Next(0, 20);
                    dl.UpdateChargeDrone(item.Id, stationDo.Id);
                }
                if (item.Status == BO.DroneStatus.Avaliable)
                {
                    IEnumerable<IDAL.DO.Parcel> p = dl.GetParcelsByPerdicate(it=> it.Delivered!=t);
                    IDAL.DO.Parcel pc = p.ElementAtOrDefault(rand.Next(0, p.Count()));
                    item.CurrentPlace = new BO.Location() { Longitude = dl.GetCustomer(pc.TargetId).Longitude, Latitude= dl.GetCustomer(pc.TargetId).Latitude };
                    stationDo = closeStation(item.CurrentPlace.Longitude, item.CurrentPlace.Latitude);
                    distance1 = shortDis(item.CurrentPlace.Longitude, item.CurrentPlace.Latitude, stationDo);
                    item.Battery = rand.NextDouble() + rand.Next((int)getMinBattery(distance1, stationDo.Id)+1, 100);
                    dl.UpdateChargeDrone(item.Id, stationDo.Id);
                }
                UpdateDroneToL(item);
            }

        }
        private IDAL.DO.BaseStation closeStation(double lon1, double lat1)
        {
            double min = 1000000000000;
            double dis;
            int id=0;
            foreach (var item in dl.ListAvaliableSlots())
            {
                dis = dl.DistanceInKm(lon1,lat1,item.Longitude, item.Latitude);
                if (dis < min)
                {
                    min = dis;
                    id = item.Id;
                }
            }
            return dl.GetBaseStation(id);
        }
        private double shortDis(double lon, double lat, IDAL.DO.BaseStation b)
        {
            return dl.DistanceInKm(lon, lat, b.Longitude,b.Latitude);
        }
        private double getMinBattery(double distance,int droneId)
        {
            BO.DroneToL droneList = GetDroneToL(droneId);
            double minBattery=-1;
            if (droneList.Status == BO.DroneStatus.Avaliable)
                minBattery= electricUse[0] * distance;
            else if (droneList.Status == BO.DroneStatus.Delivery)
            {
                switch (GetParcel(droneList.ParcelId).Weight)
                {
                    case BO.WeightCategories.Light:
                        minBattery= electricUse[1] * distance;
                        break;
                    case BO.WeightCategories.Medium:
                        minBattery = electricUse[2] * distance;
                        break;
                    case BO.WeightCategories.Heavy:
                        minBattery = electricUse[3] * distance;
                        break;
                    default:
                        break;
                }
            }
            return minBattery;
        }
    }
}
