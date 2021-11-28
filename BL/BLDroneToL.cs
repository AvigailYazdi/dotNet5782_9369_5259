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
        private void CheckDL(int Id)
        {
            if (!DList.Any(dn => dn.Id == Id))
                throw new BO.MissingIdException(Id, "Drone In List");
        }
        public BO.DroneToL GetDroneToL(int id)
        {
            CheckDL(id);
            return DList.Find(item => item.Id == id);
        }

    }
}
