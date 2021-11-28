﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    interface IDal
    {
        #region Drone
        public void AddDrone(Drone d);
        public void UpdateChargeDrone(int droneId, int baseStationId);
        public void UpdateDischargeDrone(int droneId);
        public Drone GetDrone(int id);
        public IEnumerable<Drone> ListDrone();
        public void DeleteDrone(int id);
        public IEnumerable<Drone> GetDronesByPerdicate(Predicate<Drone> predicate);
        #endregion
        #region BaseStation
        public int NumOfNotAvaliableSlots(int id);
        public void AddBaseStation(BaseStation bs);
        public BaseStation GetBaseStation(int id);
        public IEnumerable<BaseStation> ListBaseStation();
        public IEnumerable<BaseStation> ListAvaliableSlots();
        public void DeleteStation(int id);
        public IEnumerable<BaseStation> GetStationsByPerdicate(Predicate<BaseStation> predicate);

        #endregion
        #region Customer
        public void AddCustomer(Customer c);
        public Customer GetCustomer(int id);
        public IEnumerable<Customer> ListCustomer();
        public void DeleteCustomer(int id);
        public IEnumerable<Customer> GetCustomersByPerdicate(Predicate<Customer> predicate);
        #endregion
        #region Parcel
        public void AddParcel(Parcel p);
        public void UpdateParcelToDrone(int droneId, int parcelId);
        public void UpdateParcelCollect(int parcelId);
        public void UpdateParcelDelivery(int parcelId);
        public Parcel GetParcel(int id);
        public IEnumerable<Parcel> ListParcel();
        public IEnumerable<Parcel> ListNotConnected();
        public void DeleteParcel(int id);
        public IEnumerable<Parcel> GetParcelsByPerdicate(Predicate<Parcel> predicate);

        #endregion
        #region DroneCharge
        public IEnumerable<DroneCharge> GetDronesInChargeByPerdicate(Predicate<DroneCharge> predicate);
        public DroneCharge GetDroneCharge(int id);
        public void DeleteDroneCharge(int id);
        #endregion
        #region Others
        public double Distance(double lon, double lat, int choice, int id);
        public string Base60(double num);
        #endregion
    }
}
