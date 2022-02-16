using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

namespace Dal
{
    sealed partial class DalXml : IDal
    {

        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        DalXml() { }

        #region DS Xml Files
        string dronesPath = @"dronesXml.xml";
        string parcelsPath = @"parcelsXml.xml";
        string stationsPath = @"stationsXml.xml";
        string customersPath = @"customersXml.xml";
        string dronesChargePath = @"dronesChargeXml.xml";
        string usersPath = @"usersXml.xml";
        string configPath = @"config.xml";
        #endregion

    }
}
