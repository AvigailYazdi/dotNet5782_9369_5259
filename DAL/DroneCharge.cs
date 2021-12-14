using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// A struct of the drone charges.
        /// fields: drone id and station id.
        /// functions: To string- a function that returns the string to print.
        /// </summary>
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StationId { get; set; }
            public DateTime? insertTime { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
