using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Role UserRole { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
