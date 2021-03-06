using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum WeightCategories { Light, Medium, Heavy }
    public enum Priorities { Normal, Fast, Emergency }
    public enum DroneStatus { Avaliable, Maintenance, Delivery }
    public enum ParcelStatus { Created, Connected, PickedUp, Provided }
    public enum ParcelInTranStatus { WaitToCollect, OnWay }
    public enum Role { Manager, Customer }

}
