using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;
using DalApi;

namespace Dal
{
    sealed partial class DalObject : IDal
    {
        static readonly IDal instance = new DalObject();
        public static IDal Instance { get => instance; }
        /// <summary>
        /// A function that initialize the arrays.
        /// </summary>
        DalObject() { Initialize(); }
    }
}
