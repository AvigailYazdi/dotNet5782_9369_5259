using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInP Sender { get; set; }
        public CustomerInP Receiver { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneInP MyDrone { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
