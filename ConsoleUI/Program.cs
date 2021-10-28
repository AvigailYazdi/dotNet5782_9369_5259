using System;
using IDAL.DO;
namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Exit, Add, Update, ViewOne, ViewList};
        enum AddOrView { BaseStation, Drone, Customer, Parcel};
        enum UpDate { ParcelToDrone, Collect, Delivery, Charge , Discharge };
        enum ViewList { BaseStation, Drone, Customer, Parcel, NotConnected, AvaliableSlots};
       void PrintMenue()
        {
            Drone d = new Drone() ;
            Parcel p = new Parcel() ;
            BaseStation b= new BaseStation();
            Customer c = new Customer();
            MenuOptions mo;
            int option;
            bool check = int.TryParse(Console.ReadLine(), out option);
            mo = (MenuOptions)option;
            //if (check)                                         
            //    mo = (MenuOptions)option                                         
            //else                                     
            //    Console.WriteLine("ERROR");
            switch (mo)
            {
                case MenuOptions.Exit:
                    break;
                case MenuOptions.Add:
                    AddOrView aov;
                    check = int.TryParse(Console.ReadLine(), out option);
                    aov = (AddOrView)option ;
                    switch (aov)
                    {
                        case AddOrView.BaseStation:
                            Console.WriteLine("Enter id, name, longitude, lattitude and number of charge slots of a base station");
                            b.Id = Console.Read();
                            b.Name= Console.ReadLine();
                            b.Longitude= Console.Read();
                            b.Lattitude= Console.Read();
                            b.ChargeSlots= Console.Read();
                            DalObject.DalObject.AddBaseStation(b);
                            break;
                        case AddOrView.Drone:
                            Console.WriteLine("Enter id, model, max weight, status and battery of a drone");
                            d.Id= Console.Read();
                            d.Model= Console.ReadLine();
                            d.MaxWeight= (WeightCategories)Console.Read();
                            d.Status= (DroneStatuses)Console.Read();
                            d.Battery= Console.Read();
                            DalObject.DalObject.AddDrone(d);
                            break;
                        case AddOrView.Customer:
                            Console.WriteLine("Enter id, name, phone number, longitude and lattitude of a customer");
                            c.Id= Console.Read();
                            c.Name= Console.ReadLine();
                            c.Phone= Console.ReadLine();
                            c.Longitude= Console.Read();
                            c.Lattitude= Console.Read();
                            DalObject.DalObject.AddCustomer(c);
                            break;
                        case AddOrView.Parcel:
                            Console.WriteLine("Enter sender id, target id, weight and priority of a parcel");
                            p.SenderId= Console.Read();
                            p.TargetId= Console.Read();
                            p.Weight= (WeightCategories)Console.Read();
                            p.Priority= (Priorities)Console.Read();
                            p.Requested= DateTime.Now;
                            p.DroneId = 0;
                            DalObject.DalObject.AddParcel(p);
                            break;
                        default:
                            break;
                    }
                    break;
                case MenuOptions.Update:
                    int DroneId, ParcelId, BaseStationId;
                    UpDate up;
                    check = int.TryParse(Console.ReadLine(), out option);
                    up = (UpDate)option;
                    switch (up)
                    {
                        case UpDate.ParcelToDrone:
                            Console.WriteLine("Enter Drone and Parcel id");
                            DroneId = Console.Read();
                            ParcelId= Console.Read();
                            DalObject.DalObject.UpdateParcelToDrone(DroneId, ParcelId);
                            break;
                        case UpDate.Collect:
                            Console.WriteLine("Enter Parcel id");
                            ParcelId = Console.Read();
                            DalObject.DalObject.UpdateParcelCollect(ParcelId);
                            break;
                        case UpDate.Delivery:
                            Console.WriteLine("Enter Parcel id");
                            ParcelId = Console.Read();
                            DalObject.DalObject.UpdateParcelDelivery(ParcelId);
                            break;
                        case UpDate.Charge:
                            Console.WriteLine("Enter Drone and Base station id");
                            DroneId = Console.Read();
                            BaseStationId = Console.Read();
                            DalObject.DalObject.UpdateChargeDrone(DroneId, BaseStationId);
                            break;
                        case UpDate.Discharge:
                            Console.WriteLine("Enter Drone id");
                            DroneId = Console.Read();
                            DalObject.DalObject.UpdateDischargeDrone(DroneId);
                            break;
                        default:
                            break;
                    }
                    break;
                case MenuOptions.ViewOne:
                    check = int.TryParse(Console.ReadLine(), out option);
                    aov = (AddOrView)option;
                    switch (aov)
                    {
                        case AddOrView.BaseStation:
                            Console.WriteLine("Enter id of a base station");
                            b = DalObject.DalObject.ViewBaseStation(Console.Read());
                            Console.WriteLine(b);
                            break;
                        case AddOrView.Drone:
                            Console.WriteLine("Enter id of a drone");
                            d = DalObject.DalObject.ViewDrone(Console.Read());
                            Console.WriteLine(d);
                            break;
                        case AddOrView.Customer:
                            Console.WriteLine("Enter id of a customer");
                            c = DalObject.DalObject.ViewCustomer(Console.Read());
                            Console.WriteLine(c);
                            break;
                        case AddOrView.Parcel:
                            Console.WriteLine("Enter id of a parcel");
                            p = DalObject.DalObject.ViewParcel(Console.Read());
                            Console.WriteLine(p);
                            break;
                        default:
                            break;
                    }
                    break;
                case MenuOptions.ViewList:
                    ViewList vl;
                    check = int.TryParse(Console.ReadLine(), out option);
                    vl = (ViewList)option;
                    switch (vl)
                    {
                        case ViewList.BaseStation:
                            BaseStation[] temp = DalObject.DalObject.ListBaseStation();
                            for (int i = 0; i < temp.Length; i++)
                                Console.WriteLine(temp[i]);
                            break;
                        case ViewList.Drone:
                            Drone[] temp1 = DalObject.DalObject.ListDrone();
                            for (int i = 0; i < temp1.Length; i++)
                                Console.WriteLine(temp1[i]);
                            break;
                        case ViewList.Customer:
                            Customer[] temp2 = DalObject.DalObject.ListCustomer();
                            for (int i = 0; i < temp2.Length; i++)
                                Console.WriteLine(temp2[i]);
                            break;
                        case ViewList.Parcel:
                            Parcel[] temp3 = DalObject.DalObject.ListParcel();
                            for (int i = 0; i < temp3.Length; i++)
                                Console.WriteLine(temp3[i]);
                            break;
                        case ViewList.NotConnected:
                            temp = DalObject.DalObject.ListBaseStation();
                            for (int i = 0; i < temp.Length; i++)
                                Console.WriteLine(temp[i]);
                            break;
                        case ViewList.AvaliableSlots:
                            temp3 = DalObject.DalObject.ListParcel();
                            for (int i = 0; i < temp3.Length; i++)
                                Console.WriteLine(temp3[i]);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
