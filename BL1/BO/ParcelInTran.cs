using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInTran
    {
        public int Id { get; set; }
        public ParcelInTranStatus ParcelStatus { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public CustomerInP Sender { get; set; }
        public CustomerInP Receiver { get; set; }
        public Location Collection { get; set; }
        public Location Destination { get; set; }
        public double Distance { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
