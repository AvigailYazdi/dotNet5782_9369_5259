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
            try
            {
                foreach (var item in dl.ListDrone())
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
                    IEnumerable<DO.BaseStation> bs = dl.GetStationsByPerdicate(s => s.ChargeSlots != 0);
                    DO.BaseStation stationDo = bs.ElementAtOrDefault(rand.Next(0, bs.Count()));
                    d.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
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
                                DO.Customer c = dl.GetCustomer(dl.GetParcel(d.ParcelId).SenderId);
                                d.CurrentPlace = new BO.Location { Longitude = c.Longitude, Latitude = c.Latitude };
                                break;
                            case BO.ParcelStatus.PickedUp:
                                DO.Customer c1 = dl.GetCustomer(dl.GetParcel(d.ParcelId).SenderId);
                                d.CurrentPlace = new BO.Location { Longitude = c1.Longitude, Latitude = c1.Latitude };
                                break;
                            case BO.ParcelStatus.Provided:
                                DO.Customer targetDo = dl.GetCustomer(dl.GetParcel(d.ParcelId).TargetId);
                                DO.BaseStation b = closeStation(targetDo.Longitude, targetDo.Latitude);
                                d.CurrentPlace = new BO.Location { Longitude = b.Longitude, Latitude = b.Latitude };
                                break;
                            default:
                                break;
                        }
                    }
                    //else
                    //{
                    //    IEnumerable<DO.BaseStation> bs = dl.GetStationsByPerdicate(s => s.ChargeSlots != 0);
                    //    DO.BaseStation stationDo = bs.ElementAtOrDefault(rand.Next(0, bs.Count()));
                    //    d.CurrentPlace = new BO.Location() { Longitude = stationDo.Longitude, Latitude = stationDo.Latitude };
                    //}
                    dList.Add(d);
                }
            }
            catch (DO.MissingIdException ex)
            {
                throw new BO.MissingIdException(ex.Id, ex.EntityName);
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

        private int sumDigits(int num)//get: a positive integer number, return:the sum of its digits
        {
            int sum = 0;
            while (num > 0)//'while' loop foe sum of its digits
            {
                sum = sum + num % 10;
                num = num / 10;
            }
            return sum;
        }
        private int lastDigitId(int num)//get: a positive integer number with 8 digits, return:ID Check Digit
        {
            int sum = 0;
            for (int i = 8; i >= 1; i--)//'for' loop to scan over the eight digits of the number
            {
                if (i % 2 != 0)//to check if the location of the digit is odd
                    sum = sum + sumDigits(num % 10);
                else//in case that the location of the digit is even
                    sum = sum + sumDigits(num % 10 * 2);
                num = num / 10;
            }
            return (10 - sum % 10);
        }

        public bool CheckId(int id)
        {
            return id % 10 == lastDigitId((int)(id / 10));
        }
    }
}
