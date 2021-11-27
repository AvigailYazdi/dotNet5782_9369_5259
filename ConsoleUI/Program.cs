// Avigail Yazdi 213259369, Shilat Shimon 212435259
// We did both of the bonuses.
using System;
using System.Collections.Generic;
using IDAL.DO;
using DalObject;
namespace ConsoleUI
{
    /// <summary>
    /// A class of the main program that run the process of delivery company by drones
    /// </summary>
    class Program
    {
        /// <summary>
        /// enums of the options to choose an action
        /// </summary>
        enum MenuOptions { Exit, Add, Update, ViewOne, ViewList};
        enum AddOrView { BaseStation=1, Drone, Customer, Parcel};
        enum UpDate { ParcelToDrone=1, Collect, Delivery, Charge , Discharge };
        enum ViewList { BaseStation=1, Drone, Customer, Parcel, NotConnected, AvaliableSlots,viewDistance};
        /// <summary>
        /// A function that runs the process with a client
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DalObject.DalObject dal= new DalObject.DalObject();
            Drone d = new Drone() ;
            Parcel p = new Parcel() ;
            BaseStation b= new BaseStation();
            Customer c = new Customer();
            Console.WriteLine(@"Enter a number:          
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.");
            MenuOptions mo;
            int option;
            bool check = int.TryParse(Console.ReadLine(), out option);
            mo = (MenuOptions)option;
            //if (check)                                         
            //    mo = (MenuOptions)option                                         
            //else                                     
            //    Console.WriteLine("ERROR");
            while (mo!=0)
            {
                switch (mo)
                {
                    case MenuOptions.Exit:
                        break;
                    case MenuOptions.Add:
                        Console.WriteLine(@"Enter a number:          
1- To add a base station,
2- To add a drone,
3- To add a customer,
4- To add a parcel.");
                        AddOrView aov;
                        check = int.TryParse(Console.ReadLine(), out option);
                        aov = (AddOrView)option;
                        switch (aov)
                        {
                            case AddOrView.BaseStation:
                                Console.WriteLine("Enter id, name, Longitude, Lattitude and num of Charge slots:");
                                b.Id = int.Parse(Console.ReadLine());
                                b.Name = Console.ReadLine();
                                b.Longitude = Convert.ToDouble(Console.ReadLine());
                                b.Latitude = Convert.ToDouble(Console.ReadLine());
                                b.ChargeSlots = int.Parse(Console.ReadLine());
                                dal.AddBaseStation(b);
                                break;
                            case AddOrView.Drone:
                                Console.WriteLine("Enter id, model, max weight, status and battery of a drone");
                                d.Id = int.Parse(Console.ReadLine());
                                d.Model = Console.ReadLine();
                                d.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                                dal.AddDrone(d);
                                break;
                            case AddOrView.Customer:
                                Console.WriteLine("Enter id, name, phone number, longitude and lattitude of a customer");
                                c.Id = int.Parse(Console.ReadLine());
                                c.Name = Console.ReadLine();
                                c.Phone = Console.ReadLine();
                                c.Longitude = Convert.ToDouble(Console.ReadLine());
                                c.Latitude = Convert.ToDouble(Console.ReadLine());
                                dal.AddCustomer(c);
                                break;
                            case AddOrView.Parcel:
                                Console.WriteLine("Enter sender id, target id, weight and priority of a parcel");
                                p.SenderId = int.Parse(Console.ReadLine());
                                p.TargetId = int.Parse(Console.ReadLine());
                                p.Weight = (WeightCategories)int.Parse(Console.ReadLine());
                                p.Priority = (Priorities)int.Parse(Console.ReadLine());
                                p.Requested = DateTime.Now;
                                p.DroneId = 0;
                                dal.AddParcel(p);
                                break;
                            default:
                                break;
                        }
                        break;
                    case MenuOptions.Update:
                        Console.WriteLine(@"Enter a number:          
1- To update parcel to drone,
2- To update parcel collect,
3- To update parcel delivery,
4- To update charge drone,
5- To update discharge drone.");
                        int DroneId, ParcelId, BaseStationId;
                        UpDate up;
                        check = int.TryParse(Console.ReadLine(), out option);
                        up = (UpDate)option;
                        switch (up)
                        {
                            case UpDate.ParcelToDrone:
                                Console.WriteLine("Enter Drone and Parcel id");
                                DroneId = int.Parse(Console.ReadLine());
                                ParcelId = int.Parse(Console.ReadLine());
                                dal.UpdateParcelToDrone(DroneId, ParcelId);
                                break;
                            case UpDate.Collect:
                                Console.WriteLine("Enter Parcel id");
                                ParcelId = int.Parse(Console.ReadLine());
                                dal.UpdateParcelCollect(ParcelId);
                                break;
                            case UpDate.Delivery:
                                Console.WriteLine("Enter Parcel id");
                                ParcelId = int.Parse(Console.ReadLine());
                                dal.UpdateParcelDelivery(ParcelId);
                                break;
                            case UpDate.Charge:
                                Console.WriteLine("Enter Drone and Base station id");
                                DroneId = int.Parse(Console.ReadLine());
                                BaseStationId = int.Parse(Console.ReadLine());
                                dal.UpdateChargeDrone(DroneId, BaseStationId);
                                break;
                            case UpDate.Discharge:
                                Console.WriteLine("Enter Drone id");
                                DroneId = int.Parse(Console.ReadLine());
                                dal.UpdateDischargeDrone(DroneId);
                                break;
                            default:
                                break;
                        }
                        break;
                    case MenuOptions.ViewOne:
                        Console.WriteLine(@"Enter a number:          
1- To print a base station,
2- To print a drone,
3- To print a customer,
4- To print a parcel.");
                        check = int.TryParse(Console.ReadLine(), out option);
                        aov = (AddOrView)option;
                        switch (aov)
                        {
                            case AddOrView.BaseStation:
                                Console.WriteLine("Enter id of a base station");
                                b = dal.GetBaseStation(int.Parse(Console.ReadLine()));
                                Console.WriteLine(b);
                                break;
                            case AddOrView.Drone:
                                Console.WriteLine("Enter id of a drone");
                                d = dal.GetDrone(int.Parse(Console.ReadLine()));
                                Console.WriteLine(d);
                                break;
                            case AddOrView.Customer:
                                Console.WriteLine("Enter id of a customer");
                                c = dal.GetCustomer(int.Parse(Console.ReadLine()));
                                Console.WriteLine(c);
                                break;
                            case AddOrView.Parcel:
                                Console.WriteLine("Enter id of a parcel");
                                p = dal.GetParcel(int.Parse(Console.ReadLine()));
                                Console.WriteLine(p);
                                break;
                            default:
                                break;
                        }
                        break;
                    case MenuOptions.ViewList:
                        Console.WriteLine(@"Enter a number:          
1- To print all base stations,
2- To print all drones,
3- To print all customers,
4- To print all parcels,
5- To print all not- connected parcels,
6- To print all avaliable base stations,
7- To calculate a distance.");
                        ViewList vl;
                        check = int.TryParse(Console.ReadLine(), out option);
                        vl = (ViewList)option;
                        switch (vl)
                        {
                            case ViewList.BaseStation:
                                List<BaseStation> temp = new List<BaseStation>(dal.ListBaseStation());
                                foreach (BaseStation item in temp)
                                    Console.WriteLine(item);
                                break;
                            case ViewList.Drone:
                                List<Drone> temp1 = new List<Drone>(dal.ListDrone()); 
                                foreach (Drone item in temp1)
                                    Console.WriteLine(item);
                                break;
                            case ViewList.Customer:
                                List<Customer> temp2 = new List<Customer>(dal.ListCustomer());
                                foreach (Customer item in temp2)
                                    Console.WriteLine(item);
                                break;
                            case ViewList.Parcel:
                                List<Parcel> temp3 = new List<Parcel>(dal.ListParcel());
                                foreach (Parcel item in temp3)
                                    Console.WriteLine(item);
                                break;
                            case ViewList.NotConnected:
                                List<Parcel> temp4 =new List<Parcel>( dal.ListNotConnected());
                                foreach (Parcel item in temp4)
                                    Console.WriteLine(item);
                                break;
                            case ViewList.AvaliableSlots:
                                List<BaseStation> temp5 = new List<BaseStation>(dal.ListAvaliableSlots());
                                foreach (BaseStation item in temp5)
                                    Console.WriteLine(item);
                                break;
                            case ViewList.viewDistance:
                                Console.WriteLine("Enter longtutude and lattitude");
                                double lon = Convert.ToDouble(Console.ReadLine());
                                double lat = Convert.ToDouble(Console.ReadLine());
                                Console.WriteLine("Enter 0 to distance between a station and its id or between a customer and his/her id");
                                int choice = int.Parse(Console.ReadLine());
                                int id = int.Parse(Console.ReadLine());
                                Console.WriteLine("The distance is: " + dal.distance(lon, lat, choice, id));
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                Console.WriteLine(@"Enter a number:          
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.");
                check = int.TryParse(Console.ReadLine(), out option);
                mo = (MenuOptions)option;
            }
        }
    }
}
/*
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
1
Enter a number:
1- To add a base station,
2- To add a drone,
3- To add a customer,
4- To add a parcel.
1
Enter id, name,Longitude, Lattitude and num of Charge slots:
1234
abc
-36.123456
29.654321
32
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
3
Enter a number:
1- To print a base station,
2- To print a drone,
3- To print a customer,
4- To print a parcel.
1
Enter id of a base station
1234
Base station- Id: 1234, Name: abc,Longitude: 36°7'24.441''S,Lattitude: 29°39'15.555''E Charge slots: 32.
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
4
Enter a number:
1- To print all base stations,
2- To print all drones,
3- To print all customers,
4- To print all parcels,
5- To print all not- connected parcels,
6- To print all avaliable base stations,
7- To calculate a distance.
2
Drone- Id: 7401, Model: , Max Weight: Medium, Status: Delivery, Battery: 100
Drone- Id: 5353, Model: , Max Weight: Heavy, Status: Avaliable, Battery: 100
Drone- Id: 4091, Model: , Max Weight: Heavy, Status: Delivery, Battery: 100
Drone- Id: 9557, Model: , Max Weight: Heavy, Status: Delivery, Battery: 100
Drone- Id: 3803, Model: , Max Weight: Medium, Status: Maintenance, Battery: 100
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
1
Enter a number:
1- To add a base station,
2- To add a drone,
3- To add a customer,
4- To add a parcel.
2
Enter id, model, max weight, status and battery of a drone
2100
aaaa
123
1
57
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update parcel to drone,
2- To update parcel collect,
3- To update parcel delivery,
4- To update charge drone,
5- To update discharge drone.
4
Enter Drone and Base station id
2100
1234
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
4
Enter a number:
1- To print all base stations,
2- To print all drones,
3- To print all customers,
4- To print all parcels,
5- To print all not- connected parcels,
6- To print all avaliable base stations,
7- To calculate a distance.
1
Base station- Id: 4643, Name: ,Longitude: 29°18'0''E,Lattitude: 33°47'59.999''E Charge slots: 7.
Base station- Id: 7528, Name: ,Longitude: 32°53'59.999''E,Lattitude: 33°42'0''E Charge slots: 6.
Base station- Id: 1234, Name: abc,Longitude: 36°7'24.441''S,Lattitude: 29°39'15.555''E Charge slots: 31.
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update parcel to drone,
2- To update parcel collect,
3- To update parcel delivery,
4- To update charge drone,
5- To update discharge drone.
5
Enter Drone id
2100
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
3
Enter a number:
1- To print a base station,
2- To print a drone,
3- To print a customer,
4- To print a parcel.
1
Enter id of a base station
1234
Base station- Id: 1234, Name: abc,Longitude: 36°7'24.441''S,Lattitude: 29°39'15.555''E Charge slots: 32.
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
0*/
