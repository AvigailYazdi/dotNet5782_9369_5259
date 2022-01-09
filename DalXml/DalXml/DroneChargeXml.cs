using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using System.Globalization;
using System.IO;
using System.Xml.Linq;


namespace Dal
{
    sealed partial class DalXml : IDal
    {
        /// <summary>
        /// A function that returns the list of all drones in charge
        /// </summary>
        /// <returns> list of all drones in charge </returns>
        private IEnumerable<DroneCharge> DroneChargesList()
        {
            XElement droneChargesRootElem = XmlTools.LoadListFromXMLElement(dronesChargePath);

            return (from p in droneChargesRootElem.Elements()
                    select new DroneCharge()
                    {
                        DroneId = int.Parse(p.Element("DroneId").Value),
                        StationId = int.Parse(p.Element("StationId").Value),
                        insertTime = Convert.ToDateTime(p.Element("insertTime").Value)
                    }
                   );
        }
        /// <summary>
        /// A function that checks if a drone charge appears int the list
        /// </summary>
        /// <param name="id">The id of drone charge</param>
        private bool checkDc(int id)
        {
            IEnumerable<DroneCharge> dronesCharge = DroneChargesList();
            return dronesCharge.Any(b => b.DroneId == id);
        }

        /// <summary>
        /// A function that returns the sdrones in chrges that stand in a condition 
        /// </summary>
        /// <param name="predicate">The condition to check</param>
        /// <returns>A collection of the stations that stand in the condition</returns>
        public IEnumerable<DroneCharge> GetDronesInChargeByPerdicate(Predicate<DroneCharge> predicate)
        {
            IEnumerable<DroneCharge> dronesCharge = DroneChargesList();
            return from item in dronesCharge
                   where predicate(item)
                   select item;
        }

        /// <summary>
        /// A function that returns a drone charge by its id
        /// </summary>
        /// <param name="id">The id of drone charge to get</param>
        /// <returns></returns>
        public DroneCharge GetDroneCharge(int id)
        {
            List<DroneCharge> dronesCharge = DroneChargesList().ToList();
            if (!checkDc(id))
                throw new MissingIdException(id, "Drone Charge");
            return dronesCharge.Find(s => s.DroneId == id);
        }

        /// <summary>
        /// A function that deletes a drone charge from the list
        /// </summary>
        /// <param name="id"> The id of the drone charge to delete </param>
        public void DeleteDroneCharge(int id)
        {
            //XElement dronesChargeRootElem = XmlTools.LoadListFromXMLElement(dronesChargePath);
            //List<DroneCharge> dronesCharge = DroneChargesList().ToList();
            //if (!checkDc(id))
            //    throw new MissingIdException(id, "Drone Charge");
            //dronesCharge.RemoveAll(dc => dc.DroneId == id);
            //XmlTools.SaveListToXMLElement(dronesChargeRootElem, dronesChargePath);
            XElement dronesChargeRootElem = XmlTools.LoadListFromXMLElement(dronesChargePath);

            XElement per = (from p in dronesChargeRootElem.Elements()
                            where int.Parse(p.Element("DroneId").Value) == id
                            select p).FirstOrDefault();

            if (per != null)
            {
                per.Remove(); 

                XmlTools.SaveListToXMLElement(dronesChargeRootElem, dronesChargePath);
            }
            else
                throw new MissingIdException(id, "Drone Charge");
        }
    }
}
