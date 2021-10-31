using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    /// enums of the options.
    /// WeightCategories- options for the weight of a parcel.
    /// DroneStatuses- options for the status of the drones.
    /// Priorities- options for the delivery priority.
    /// </summary>
    namespace DO
    {
        public enum WeightCategories { Light, Medium, Heavy }
        public enum DroneStatuses { Avaliable, Maintenance, Delivery }
        public enum Priorities { Normal, Fast, Emergency }
    }  
}
