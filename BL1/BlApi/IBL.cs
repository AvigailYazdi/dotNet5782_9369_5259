using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBL
    {
        #region Drone
        public void AddDrone(BO.Drone drone, int StationId);
        public void UpdateDroneName(int id, string name);
        public void UpdateDroneToCharge(int id);
        public void UpdateDisChargeDrone(int id);
        public BO.Drone GetDrone(int id);
        public IEnumerable<BO.DroneToL> DroneList();
        public IEnumerable<BO.DroneToL> GetDronesByPerdicate(Predicate<BO.DroneToL> predicate);
        #endregion
        #region Parcel
        public void AddParcel(BO.Parcel parcel);
        public void UpdateParcelToDrone(int id);
        public void UpdateParcelCollect(int id);
        public void UpdateParcelProvide(int id);
        public BO.Parcel GetParcel(int id);
        public IEnumerable<BO.ParcelToL> ParcelList();
        public IEnumerable<BO.Parcel> NotConnectedParcelList();
        #endregion
        #region Customer
        public void AddCustomer(BO.Customer customer);
        public void UpdateCustomer(int id, string name, string phoneNum);
        public BO.Customer GetCustomer(int id);
        public IEnumerable<BO.CustomerToL> CustomerList();
        #endregion
        #region Station
        public void AddStation(BO.BaseStation station);
        public void UpdateStation(int id, string name, int numSlots);
        public BO.BaseStation GetStation(int id);
        public IEnumerable<BO.StationToL> StationList();
        public IEnumerable<BO.StationToL> AvaliableStationList();
        public IEnumerable<BO.StationToL> GetStationToLByPredicate(Predicate<BO.StationToL> predicate);
        #endregion
        #region DroneToL
        public BO.DroneToL GetDroneToL(int id);
        public void DeleteDroneToL(int id);
        public void UpdateDroneToL(BO.DroneToL d);
        public void AddDroneToL(BO.DroneToL d);

        #endregion
    }
}
