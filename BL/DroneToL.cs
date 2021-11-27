using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneToL
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public double Battery { get; set; }
            public DroneStatus Status { get; set; }
            public Location CurrentPlace { get; set; }
            public int ParcelId { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
