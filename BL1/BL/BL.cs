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
        internal static Random rand;
        internal List<BO.DroneToL> dList;
        internal double[] electricUse;

        internal IDal dl = DalFactory.GetDal();

        static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }
        /// <summary>
        /// A constructor
        /// </summary>
        public BL()
        {
            rand = new Random(DateTime.Now.Millisecond);
            electricUse = dl.ElectricUse();//electric use
            dList = new List<BO.DroneToL>();

            BO.DroneToL d;
            foreach (var item in dl.ListDrone())
            {
                try
                {
                    d = new BO.DroneToL()
                    {
                        Id = item.Id,
                        Model = item.Model,
                        Weight = (BO.WeightCategories)item.MaxWeight,
                        Battery = rand.Next(20 * 100, 40 * 100) / 100.0,
                        Status = BO.DroneStatus.Avaliable,
                        CurrentPlace = new BO.Location(),
                        ParcelId = dl.GetConnectParcel(item.Id)
                    };
                    if (dl.checkDc(item.Id))
                    {
                        d.Status = BO.DroneStatus.Maintenance;
                        BO.Location c = GetStation(dl.GetDroneCharge(item.Id).StationId).Place;
                        d.CurrentPlace = new BO.Location { Longitude = c.Longitude, Latitude = c.Latitude };
                    }
                    else if (d.ParcelId != -1)
                    {
                        d.Status = BO.DroneStatus.Delivery;
                        switch (GetParcelStatus(d.ParcelId))
                        {
                            case BO.ParcelStatus.Connected:
                                BO.Location c= GetCustomer(GetParcel(d.ParcelId).Sender.Id).Place;
                                d.CurrentPlace = new BO.Location { Longitude = c.Longitude, Latitude = c.Latitude };
                                break;
                            case BO.ParcelStatus.PickedUp:
                                BO.Location c1 = GetCustomer(GetParcel(d.ParcelId).Sender.Id).Place;
                                d.CurrentPlace = new BO.Location { Longitude = c1.Longitude, Latitude = c1.Latitude };
                                break;
                            case BO.ParcelStatus.Provided:
                                DO.Customer targetDo = dl.GetCustomer(GetParcel(d.ParcelId).Receiver.Id);
                                DO.BaseStation b = closeStation(targetDo.Longitude, targetDo.Latitude);
                                d.CurrentPlace = new BO.Location { Longitude = b.Longitude, Latitude = b.Latitude };
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        IEnumerable<DO.BaseStation> bs = dl.GetStationsByPerdicate(s => s.ChargeSlots != 0);
                        DO.BaseStation stationDo = bs.ElementAtOrDefault(rand.Next(0, bs.Count()));
                        d.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                    }
                    dList.Add(d);
                }
                catch (DO.MissingIdException ex)
                {
                    throw new BO.MissingIdException(ex.Id, ex.EntityName);
                }
            }


                //dList = (from item in dl.ListDrone()//dList
                //         select new BO.DroneToL()
                //         {
                //             Id = item.Id,
                //             Model = item.Model,
                //             Weight = (BO.WeightCategories)item.MaxWeight,
                //             Battery = rand.Next(20 * 100, 40 * 100) / 100.0,
                //             Status = BO.DroneStatus.Avaliable,
                //             CurrentPlace = new BO.Location(),
                //             ParcelId = dl.GetConnectParcel(item.Id)
                //         }).ToList();
                //DO.BaseStation stationDo;
                //double distance1, distance2;
                //try
                //{
                //    foreach (var item in dList.ToList())
                //    {
                //        BO.DroneToL temp = GetDroneToL(item.Id);
                //        if (temp.ParcelId > 0 && dl.GetParcel(temp.ParcelId).Delivered == null)
                //        {
                //            DO.Parcel p = dl.GetParcel(temp.ParcelId);
                //            temp.Status = BO.DroneStatus.Delivery;
                //            DO.Customer senderDo = dl.GetCustomer(p.SenderId);
                //            DO.Customer targetDo = dl.GetCustomer(p.TargetId);
                //            if (p.PickedUp == null)
                //            {
                //                stationDo = closeStation(senderDo.Longitude, senderDo.Latitude);
                //                temp.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                //            }
                //            else
                //                temp.CurrentPlace = new BO.Location() { Longitude = senderDo.Longitude, Latitude = senderDo.Latitude };
                //            distance1 = dl.DistanceInKm(temp.CurrentPlace.Longitude, temp.CurrentPlace.Latitude, targetDo.Longitude, targetDo.Latitude);
                //            stationDo = closeStation(targetDo.Longitude, targetDo.Latitude);
                //            distance2 = dl.DistanceInKm(targetDo.Longitude, targetDo.Latitude, stationDo.Longitude, stationDo.Latitude);
                //            temp.Battery = rand.Next((int)(getBattery(distance1 + distance2, temp.Id) * 100), 100 * 100) / 100.0;
                //        }
                //        else if (temp.ParcelId <= 0)
                //        {
                //            temp.Status = (BO.DroneStatus)rand.Next(0, 2);
                //        }
                //        if (temp.Status == BO.DroneStatus.Maintenance)
                //        {
                //            IEnumerable<DO.BaseStation> bs = dl.GetStationsByPerdicate(s => s.ChargeSlots != 0);
                //            stationDo = bs.ElementAtOrDefault(rand.Next(0, bs.Count()));
                //            temp.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                //            temp.Battery = rand.Next(0, 20 * 100) / 100.0;
                //            dl.UpdateChargeDrone(temp.Id, stationDo.Id);
                //        }
                //        else if (temp.Status == BO.DroneStatus.Avaliable)
                //        {
                //            IEnumerable<DO.BaseStation> bs = dl.GetStationsByPerdicate(s => s.ChargeSlots != 0);
                //            stationDo = bs.ElementAtOrDefault(rand.Next(0, bs.Count()));
                //            temp.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                //            IEnumerable<DO.Parcel> p = dl.GetParcelsByPerdicate(it => it.Delivered != null);
                //            if (p.Count() != 0)
                //            {
                //                DO.Parcel pc = p.ElementAtOrDefault(rand.Next(0, p.Count()));
                //                temp.CurrentPlace = new BO.Location() { Longitude = dl.GetCustomer(pc.TargetId).Longitude, Latitude = dl.GetCustomer(pc.TargetId).Latitude };
                //                stationDo = closeStation(temp.CurrentPlace.Longitude, temp.CurrentPlace.Latitude);
                //                distance1 = dl.DistanceInKm(temp.CurrentPlace.Longitude, temp.CurrentPlace.Latitude, stationDo.Longitude, stationDo.Latitude);
                //                temp.Battery = rand.Next((int)(getBattery(distance1, stationDo.Id) * 100), 100 * 100) / 100.0;
                //                // dl.UpdateChargeDrone(temp.Id, stationDo.Id);
                //            }
                //        }
                //        UpdateDroneToL(temp);
                //    }
                //}
                //catch (DO.MissingIdException ex)

                //{
                //    throw new BO.MissingIdException(ex.Id, ex.EntityName);
                //}

            }
        /// <summary>
        /// A function that returns the station is the closet to the location
        /// </summary>
        /// <param name="lon1">the longitude</param>
        /// <param name="lat1">the latitude</param>
        /// <returns>the closet station</returns>
        private DO.BaseStation closeStation(double lon1, double lat1)
        {
            double min = 1000000000000;
            double dis;
            int id = 0;
            foreach (var item in dl.GetStationsByPerdicate(s => s.ChargeSlots != 0))
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
            catch (DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
            }

        }
        /// <summary>
        /// A function that colculates the wasted batttery
        /// </summary>
        /// <param name="distance">distance</param>
        /// <param name="droneId">the drone </param>
        /// <returns></returns>
        private double getBattery(double distance, int droneId)
        {
            BO.DroneToL droneList = GetDroneToL(droneId);
            double battery = -1;
            if (droneList.Status == BO.DroneStatus.Avaliable)
                battery = electricUse[0] * distance;
            else if (droneList.Status == BO.DroneStatus.Delivery)
            {
                switch (GetParcel(droneList.ParcelId).Weight)
                {
                    case BO.WeightCategories.Light:
                        battery = electricUse[1] * distance;
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
            return ((int)(battery * 100)) / 100.0;
        }
        /// <summary>
        /// A funciton that returns the battery that will be wasted in the way
        /// </summary>
        /// <param name="distance"> The distance in KM </param>
        /// <param name="parcelId"> The id of the parcel</param>
        /// <returns> The battery</returns>
        private double getWasteBattery(double distance, int parcelId)
        {
            double battery = -1;
            if (parcelId == -1)
                battery = electricUse[0] * distance;
            else
            {
                BO.Parcel parcel = GetParcel(parcelId);
                switch (parcel.Weight)
                {
                    case BO.WeightCategories.Light:
                        battery = electricUse[1] * distance;
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
            return battery;
        }
    }
}
