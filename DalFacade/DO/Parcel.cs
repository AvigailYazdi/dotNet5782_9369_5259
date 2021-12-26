using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// A struct of the parcels.
    /// fields: id, sender id, target id, weight, priority, requested time,
    /// drone id, scheduled time, picked up time and delivered time.
    /// functions: To string- a function that returns the string to print.
    /// </summary>
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime? Requested { get; set; }
        public int DroneId { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
