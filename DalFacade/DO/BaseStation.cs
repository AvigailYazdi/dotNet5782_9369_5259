using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// A struct of the base stations.
    /// fields: id, name, Longitude, Lattitude and number of empty charge slots.
    /// functions: To string- a function that returns the string to print.
    /// </summary>
    public struct BaseStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ChargeSlots { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
