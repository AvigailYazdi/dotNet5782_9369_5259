using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToL
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string PhoneNum { get; set; }
            public int NumArrived { get; set; }
            public int NumSend { get; set; }
            public int NumGot { get; set; }
            public int NumOnWay { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
