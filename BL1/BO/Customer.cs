using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public Location Place { get; set; }
        public IEnumerable<ParcelAtC> SendParcel { get; set; }
        public IEnumerable<ParcelAtC> GetParcel { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
