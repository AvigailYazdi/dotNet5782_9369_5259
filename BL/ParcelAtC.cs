using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enums;

namespace IBL
{
    namespace BO
    {
        public class ParcelAtC
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatus Status { get; set; }
            public CustomerInP OtherC { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
