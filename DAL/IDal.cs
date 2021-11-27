using System;
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
        public void UpdateChargeDrone(int DroneId, int BaseStationId);
        public void UpdateDischargeDrone(int DroneId);
        public Drone GetDrone(int Id);
        public IEnumerable<Drone> ListDrone();
        public void DeleteDrone(int id);
        public IEnumerable<Drone> GetDronesByPerdicate(Predicate<Drone> predicate);
        #endregion
        #region BaseStation
        public int numOfNotAvaliableSlots(int id);
        public void AddBaseStation(BaseStation bs);
        public BaseStation GetBaseStation(int Id);
        public IEnumerable<BaseStation> ListBaseStation();
        public IEnumerable<BaseStation> ListAvaliableSlots();
        public void DeleteStation(int id);
        public IEnumerable<BaseStation> GetStationsByPerdicate(Predicate<BaseStation> predicate);
        #endregion
        #region Customer
        public void AddCustomer(Customer c);
        public Customer GetCustomer(int Id);
        public IEnumerable<Customer> ListCustomer();
        public void DeleteCustomer(int Id);
        public IEnumerable<Customer> GetCustomersByPerdicate(Predicate<Customer> predicate);
        #endregion
        #region Parcel
        public void AddParcel(Parcel p);
        public void UpdateParcelToDrone(int DroneId, int ParcelId);
        public void UpdateParcelCollect(int ParcelId);
        public void UpdateParcelDelivery(int ParcelId);
        public Parcel GetParcel(int Id);
        public IEnumerable<Parcel> ListParcel();
        public IEnumerable<Parcel> ListNotConnected();
        public void DeleteParcel(Parcel p);
        public IEnumerable<Parcel> GetParcelsByPerdicate(Predicate<Parcel> predicate);

        #endregion
        #region Others
        public double distance(double lon, double lat, int choice, int id);
        public string base60(double num);
        #endregion
    }
}
