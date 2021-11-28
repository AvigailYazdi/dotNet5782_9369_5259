using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using static DalObject.DataSource;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        private object other;
        /// <summary>
        /// A function that changes the number to representation at base 60
        /// </summary>
        /// <param name="num"> the number to change </param>
        /// <returns> returns the number number at base 60 </returns>
        public string Base60(double num)///////////////////////////////////////////////////
        {
            string str = Math.Abs((int)num) + "°";
            num = (num - (int)num) * 60.0;
            str += (Math.Abs((int)num) + "'");
            num = (int)((num - (int)num) * 60000.0) / 1000.0;
            str += Math.Abs(num) + "''";
            if (num < 0)
                return str + "S";
            return str + "E";
        }
        /// <summary>
        /// A function that calculates the distance 
        /// </summary>
        /// <param name="lon"> the longtitude</param>
        /// <param name="lat">the lattitude</param>
        /// <param name="choice">the choice between a customer or base station</param>
        /// <param name="id"> the id of base station or customer</param>
        /// <returns>the distance</returns>
        public double Distance(double lon, double lat, int choice, int id)
        {
            double dis = 0;
            if (choice == 0)// base station
            {
                BaseStation b = stations.Find(s => s.Id == id);
                dis = Math.Sqrt(Math.Pow(lon - b.Longitude, 2) + Math.Pow(lat - b.Latitude, 2));
            }
            else // customer
            {
                Customer c = customers.Find(s => s.Id == id);
                dis = Math.Sqrt(Math.Pow(lon - c.Longitude, 2) + Math.Pow(lat - c.Latitude, 2));
            }
            return dis;
        }
        public double [] ElectricUse()
        {
            double[] arr = new double[] { config.avaliable, config.light, config.medium, config.heavy, config.batteryCharge };
            return arr;
        }
    }
}
