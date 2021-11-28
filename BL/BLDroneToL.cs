using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public partial class BL : IBL
    {
        private object droneToL;
        /// <summary>
        /// A function that checks if the id appears in the list
        /// </summary>
        /// <param name="id">The id to check</param>
        private void checkDL(int id)
        {
            if (!dList.Any(dn => dn.Id == id))
                throw new BO.MissingIdException(id, "Drone In List");
        }
        /// <summary>
        /// A function that returns a dron  to list
        /// </summary>
        /// <param name="id">The id of a srone to list to get</param>
        /// <returns></returns>
        public BO.DroneToL GetDroneToL(int id)
        {
            checkDL(id);
            return dList.Find(item => item.Id == id);
        }

    }
}
