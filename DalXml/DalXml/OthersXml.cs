using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;

namespace Dal
{
    sealed partial class DalXml : IDal
    {

        //private object other;
        /// <summary>
        /// A function that changes the number to representation at base 60
        /// </summary>
        /// <param name="num"> the number to change </param>
        /// <returns> returns the number number at base 60 </returns>
        public string Base60(double num)
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
        /// <param name="lat1"> the lattitude of 1</param>
        /// <param name="lon1">the longtitude of 1</param>
        /// <param name="lat2">the lattitude of 2</param>
        /// <param name="lon2"> the longtitude of 2</param>
        /// <returns>the distance</returns>
        public double DistanceInKm(double lat1, double lon1, double lat2, double lon2)
        {
            int R = 6371; // Radius of the earth in km
            double dLat = deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = deg2rad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return (int)(d * 1000) / 1000.0;
        }

        /// <summary>
        /// A function that is part of the function 'DistanceInKm'
        /// </summary>
        /// <param name="deg">the remainder between two numbers</param>
        /// <returns>part of the above function</returns>
        private double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        /// <summary>
        /// A function that returns the static properties
        /// </summary>
        /// <returns>an array of the properties</returns>
        public double[] ElectricUse()
        {
            double[] arr = { 1, 9 };
            //double avaliable = double.Parse(XElement.Load(configPath).Element("avaliable").Value);
            //double light= double.Parse(XElement.Load(configPath).Element("light").Value);
            //double medium = double.Parse(XElement.Load(configPath).Element("medium").Value);
            //double heavy = double.Parse(XElement.Load(configPath).Element("heavy").Value);
            //double batteryCharge = double.Parse(XElement.Load(configPath).Element("batteryCharge").Value);
            //double[] arr = new double[] { avaliable, light, medium, heavy, batteryCharge };
            return arr;
        }
    }
}
