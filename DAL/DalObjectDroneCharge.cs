using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using static DalObject.DataSource;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        private object droneCharge;
        /// <summary>
        /// A function that checks if a drone charge appears int the list
        /// </summary>
        /// <param name="id">The id of drone charge</param>
        private bool checkDc(int id)
        {
            return dronesCharge.Any(b => b.DroneId == id);
        }
        /// <summary>
        /// A function that returns the sdrones in chrges that stand in a condition 
        /// </summary>
        /// <param name="predicate">The condition to check</param>
        /// <returns>A collection of the stations that stand in the condition</returns>
        public IEnumerable<DroneCharge> GetDronesInChargeByPerdicate(Predicate<DroneCharge> predicate)
        {
            return from item in dronesCharge
                   where predicate(item)
                   select item;
        }
        /// <summary>
        /// A function that returns a drone charge by its id
        /// </summary>
        /// <param name="id">The id of drone charge to get</param>
        /// <returns></returns>
        public DroneCharge GetDroneCharge(int id)
        {
            if(!checkDc(id))
                throw new MissingIdException(id, "Drone Charge");
            return dronesCharge.Find(s => s.DroneId == id);
        }
        /// <summary>
        /// A function that deletes a drone charge from the list
        /// </summary>
        /// <param name="id"> The id of the drone charge to delete </param>
        public void DeleteDroneCharge(int id)
        {
            if(!checkDc(id))
                throw new MissingIdException(id, "Drone Charge");
            dronesCharge.RemoveAll(dc=>dc.DroneId==id);
        }
    }
}
