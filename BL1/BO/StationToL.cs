using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationToL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvaliableSlots { get; set; }
        public int DisAvaliableSlots { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
