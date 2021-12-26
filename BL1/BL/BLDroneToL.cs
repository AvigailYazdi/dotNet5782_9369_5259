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
        private object droneToL;
        /// <summary>
        /// A function that checks if the id appears in the list
        /// </summary>
        /// <param name="id">The id to check</param>
        private bool checkDL(int id)
        {
            return dList.Any(dn => dn.Id == id);
        }
        /// <summary>
        /// A function that returns a dron  to list
        /// </summary>
        /// <param name="id">The id of a srone to list to get</param>
        /// <returns></returns>
        public BO.DroneToL GetDroneToL(int id)
        {
            if(!checkDL(id))
                throw new BO.MissingIdException(id, "Drone In List");
            return dList.Find(item => item.Id == id);
        }
        /// <summary>
        /// A function that removes a drone in the list
        /// </summary>
        /// <param name="id">the id of a drone to delete</param>
        public void DeleteDroneToL(int id)
        {
            if (!checkDL(id))
                throw new BO.MissingIdException(id, "Drone In List");
            dList.RemoveAll(d => d.Id == id);
        }
        /// <summary>
        /// A function thet returns the index of the drone in the list
        /// </summary>
        /// <param name="id"> Id of a drone</param>
        /// <returns> The index of the drone</returns>
        private int indexDroneToL(int id)
        {
            for (int i = 0; i < dList.Count(); i++)
            {
                if (dList[i].Id == id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// A function that updates a drone
        /// </summary>
        /// <param name="d">The updated drone</param>
        public void UpdateDroneToL(BO.DroneToL d)
        {
            int i = indexDroneToL(d.Id);
            dList[i] = d;
            //DeleteDroneToL(d.Id);
            //AddDroneToL(d);
        }
        /// <summary>
        ///  A function that adds a drone to the list
        /// </summary>
        /// <param name="d"> the drone to add </param>
        public void AddDroneToL(BO.DroneToL d)
        {
            if (checkDL(d.Id))
                throw new BO.DuplicateIdException(d.Id, "Drone To List");
            dList.Add(d);
        }

    }
}
