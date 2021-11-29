using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToL
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string ReceiverName { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatus Status { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
