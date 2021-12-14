using System;
using System.Collections.Generic;
using IBL.BO;
namespace ConsoleUI_BL
{
    class Program
    {
        enum MenuOptions { Exit, Add, Update, ViewOne, ViewList };
        enum AddOrView { BaseStation = 1, Drone, Customer, Parcel };
        enum UpDate { DroneName = 1, BaseStation, Customer, ChargeDrone, DisChargeDrone, ParcelToDrone, ParcelCollect, ParcelDelivered };
        enum ViewList { BaseStation = 1, Drone, Customer, Parcel, NotConnected, AvaliableSlots };
        static void Main(string[] args)
        {
            IBL.BL bl = new IBL.BL();
            Drone d = new Drone();
            Parcel p = new Parcel();
            BaseStation b = new BaseStation();
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
            while (mo != 0)
            {
                try
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
                                    Console.WriteLine("Enter id, name, location and num of Charge slots:");
                                    b.Id = int.Parse(Console.ReadLine());
                                    b.Name = Console.ReadLine();
                                    b.Place = new Location();
                                    b.Place.Longitude = Convert.ToDouble(Console.ReadLine());
                                    b.Place.Latitude = Convert.ToDouble(Console.ReadLine());
                                    b.AvaliableSlots = int.Parse(Console.ReadLine());
                                    bl.AddStation(b);
                                    break;
                                case AddOrView.Drone:
                                    Console.WriteLine("Enter id, model and max weight of a drone");
                                    d.Id = int.Parse(Console.ReadLine());
                                    d.Model = Console.ReadLine();
                                    d.Weight = (WeightCategories)int.Parse(Console.ReadLine());
                                    bl.AddDrone(d);
                                    break;
                                case AddOrView.Customer:
                                    Console.WriteLine("Enter id, name, phone number, and location of a customer");
                                    c.Id = int.Parse(Console.ReadLine());
                                    c.Name = Console.ReadLine();
                                    c.PhoneNum = Console.ReadLine();
                                    c.Place = new Location();
                                    c.Place.Longitude = Convert.ToDouble(Console.ReadLine());
                                    c.Place.Latitude = Convert.ToDouble(Console.ReadLine());
                                    bl.AddCustomer(c);
                                    break;
                                case AddOrView.Parcel:
                                    Console.WriteLine("Enter sender id, receiver id, weight and priority of a parcel");
                                    p.Sender = new CustomerInP();
                                    p.Sender.Id = int.Parse(Console.ReadLine());
                                    p.Receiver = new CustomerInP();
                                    p.Receiver.Id = int.Parse(Console.ReadLine());
                                    p.Weight = (WeightCategories)int.Parse(Console.ReadLine());
                                    p.Priority = (Priorities)int.Parse(Console.ReadLine());
                                    bl.AddParcel(p);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case MenuOptions.Update:
                            Console.WriteLine(@"Enter a number:          
1- To update drone model,
2- To update a base station,
3- To update a customer,
4- To update drone to charge,
5- To update discharge drone,
6- To update parcel to drone,
7- To update parcel collect,
8- To update parcel delivered.");
                            int Id, NumSlots;
                            string Model, Name, phoneNum;
                            UpDate up;
                            check = int.TryParse(Console.ReadLine(), out option);
                            up = (UpDate)option;
                            switch (up)
                            {
                                case UpDate.DroneName:
                                    Console.WriteLine("Enter Drone id and model");
                                    Id = int.Parse(Console.ReadLine());
                                    Model = Console.ReadLine();
                                    bl.UpdateDroneName(Id, Model);
                                    break;
                                case UpDate.BaseStation:
                                    Console.WriteLine("Enter id, name and num slots of a base station");
                                    Id = int.Parse(Console.ReadLine());
                                    Name = Console.ReadLine();
                                    int.TryParse(Console.ReadLine(), out NumSlots);
                                    bl.UpdateStation(Id, Name, NumSlots);
                                    break;
                                case UpDate.Customer:
                                    Console.WriteLine("Enter id, name and phone number of a customer");
                                    Id = int.Parse(Console.ReadLine());
                                    Name = Console.ReadLine();
                                    phoneNum = Console.ReadLine();
                                    bl.UpdateCustomer(Id, Name, phoneNum);
                                    break;
                                case UpDate.ChargeDrone:
                                    Console.WriteLine("Enter drone id");
                                    Id = int.Parse(Console.ReadLine());
                                    bl.UpdateDroneToCharge(Id);
                                    break;
                                case UpDate.DisChargeDrone:
                                    Console.WriteLine("Enter drone id");
                                    Id = int.Parse(Console.ReadLine());
                                    bl.UpdateDisChargeDrone(Id);
                                    break;
                                case UpDate.ParcelToDrone:
                                    Console.WriteLine("Enter drone id");
                                    Id = int.Parse(Console.ReadLine());
                                    bl.UpdateParcelToDrone(Id);
                                    break;
                                case UpDate.ParcelCollect:
                                    Console.WriteLine("Enter drone id");
                                    Id = int.Parse(Console.ReadLine());
                                    bl.UpdateParcelCollect(Id);
                                    break;
                                case UpDate.ParcelDelivered:
                                    Console.WriteLine("Enter drone id");
                                    Id = int.Parse(Console.ReadLine());
                                    bl.UpdateParcelProvide(Id);
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
                                    b = bl.GetStation(int.Parse(Console.ReadLine()));
                                    Console.WriteLine(b);
                                    Console.WriteLine("The drones in charge:");
                                    foreach (DroneInCharge i in b.DroneSlots)
                                        Console.Write(i.Id + " ");
                                    Console.WriteLine();
                                    break;
                                case AddOrView.Drone:
                                    Console.WriteLine("Enter id of a drone");
                                    d = bl.GetDrone(int.Parse(Console.ReadLine()));
                                    Console.WriteLine(d); 
                                    Console.WriteLine();
                                    break;
                                case AddOrView.Customer:
                                    Console.WriteLine("Enter id of a customer");
                                    c = bl.GetCustomer(int.Parse(Console.ReadLine()));
                                    Console.WriteLine(c);
                                    Console.WriteLine("The sent parcels:");
                                    foreach (ParcelAtC i in c.SendParcel)
                                        Console.Write(i.Id + " ");
                                    Console.WriteLine();
                                    Console.WriteLine("The recieved parcels:");
                                    foreach (ParcelAtC i in c.GetParcel)
                                        Console.Write(i.Id + " ");
                                    Console.WriteLine();
                                    break;
                                case AddOrView.Parcel:
                                    Console.WriteLine("Enter id of a parcel");
                                    p = bl.GetParcel(int.Parse(Console.ReadLine()));
                                    Console.WriteLine(p);
                                    Console.WriteLine();
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
6- To print all avaliable base stations.");
                            ViewList vl;
                            check = int.TryParse(Console.ReadLine(), out option);
                            vl = (ViewList)option;
                            switch (vl)
                            {
                                case ViewList.BaseStation:
                                    IEnumerable<StationToL> temp = bl.StationList();
                                    foreach (StationToL item in temp)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    Console.WriteLine();
                                    break;
                                case ViewList.Drone:
                                    IEnumerable<DroneToL> temp1 = bl.DroneList();
                                    foreach (DroneToL item in temp1)
                                        Console.WriteLine(item);
                                    Console.WriteLine();
                                    break;
                                case ViewList.Customer:
                                    IEnumerable<CustomerToL> temp2 = bl.CustomerList();
                                    foreach (CustomerToL item in temp2)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    Console.WriteLine();
                                    break;
                                case ViewList.Parcel:
                                    IEnumerable<ParcelToL> temp3 = bl.ParcelList();
                                    foreach (ParcelToL item in temp3)
                                        Console.WriteLine(item);
                                    Console.WriteLine();
                                    break;
                                case ViewList.NotConnected:
                                    IEnumerable<Parcel> temp4 = bl.NotConnectedParcelList();
                                    foreach (Parcel item in temp4)
                                        Console.WriteLine(item);
                                    Console.WriteLine();
                                    break;
                                case ViewList.AvaliableSlots:
                                    IEnumerable<BaseStation> temp5 = bl.AvaliableStationList();
                                    foreach (BaseStation item in temp5)
                                        Console.WriteLine(item);
                                    Console.WriteLine();
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
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
2
Enter id, model, max weight and station num of a drone
123
aaa
1
1
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
2
Enter id of a drone
123

Id: 123
Model: aaa
Weight: Medium
Battery: 21.49
Status: Maintenance
MyParcel:
CurrentPlace:
Longitude: 29.7
Latitude: 33.9
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update drone model,
2- To update a base station,
3- To update a customer,
4- To update drone to charge,
5- To update discharge drone,
6- To update parcel to drone,
7- To update parcel collect,
8- To update parcel delivered.
5
Enter drone id and Charging time
123
2
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
2
Enter id of a drone
123

Id: 123
Model: aaa
Weight: Medium
Battery: 100
Status: Avaliable
MyParcel:
CurrentPlace:
Longitude: 29.7
Latitude: 33.9
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
4
Enter sender id, receiver id, weight and priority of a parcel
3
5
1
1
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update drone model,
2- To update a base station,
3- To update a customer,
4- To update drone to charge,
5- To update discharge drone,
6- To update parcel to drone,
7- To update parcel collect,
8- To update parcel delivered.
6
Enter drone id
123
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
2
Enter id of a drone
123

Id: 123
Model: aaa
Weight: Medium
Battery: 100
Status: Delivery
MyParcel:
Id: 9
ParcelStatus: WaitToCollect
Weight: Medium
Priority: Emergency
Sender:
Id: 3
Name:
Receiver:
Id: 8
Name:
Collection:
Longitude: 32.2
Latitude: 33.7
Destination:
Longitude: 33.4
Latitude: 34
Distance: 136.348
CurrentPlace:
Longitude: 29.7
Latitude: 33.9
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
2
Enter id of a drone
123

Id: 123
Model: aaa
Weight: Medium
Battery: 100
Status: Delivery
MyParcel:
Id: 9
ParcelStatus: WaitToCollect
Weight: Medium
Priority: Emergency
Sender:
Id: 3
Name:
Receiver:
Id: 8
Name:
Collection:
Longitude: 32.2
Latitude: 33.7
Destination:
Longitude: 33.4
Latitude: 34
Distance: 136.348
CurrentPlace:
Longitude: 29.7
Latitude: 33.9
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
3
Enter id of a customer
3

Id: 3
Name:
PhoneNum:
Place:
Longitude: 32.2
Latitude: 33.7
SendParcel: System.Linq.Enumerable+WhereSelectListIterator`2[IDAL.DO.Parcel,IBL.BO.ParcelAtC]
GetParcel: System.Linq.Enumerable+WhereSelectListIterator`2[IDAL.DO.Parcel,IBL.BO.ParcelAtC]
The sent parcels:
5 9 11
The recieved parcels:

Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update drone model,
2- To update a base station,
3- To update a customer,
4- To update drone to charge,
5- To update discharge drone,
6- To update parcel to drone,
7- To update parcel collect,
8- To update parcel delivered.
1
Enter Drone id and model
123
bbb
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
2
Enter id of a drone
123

Id: 123
Model: bbb
Weight: Medium
Battery: 100
Status: Delivery
MyParcel:
Id: 9
ParcelStatus: WaitToCollect
Weight: Medium
Priority: Emergency
Sender:
Id: 3
Name:
Receiver:
Id: 8
Name:
Collection:
Longitude: 32.2
Latitude: 33.7
Destination:
Longitude: 33.4
Latitude: 34
Distance: 136.348
CurrentPlace:
Longitude: 29.7
Latitude: 33.9
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update drone model,
2- To update a base station,
3- To update a customer,
4- To update drone to charge,
5- To update discharge drone,
6- To update parcel to drone,
7- To update parcel collect,
8- To update parcel delivered.
7
Enter drone id
123
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
4
Enter id of a parcel
9

Id: 9
Sender:
Id: 3
Name:
Receiver:
Id: 8
Name:
Weight: Medium
Priority: Emergency
MyDrone:
Id: 123
Battery: 72.14
CurrentPlace:
Longitude: 32.2
Latitude: 33.7
Requested: 02/12/2021 23:12:34
Scheduled: 02/12/2021 23:15:53
PickedUp: 02/12/2021 23:18:17
Delivered: 01/01/0001 00:00:00
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update drone model,
2- To update a base station,
3- To update a customer,
4- To update drone to charge,
5- To update discharge drone,
6- To update parcel to drone,
7- To update parcel collect,
8- To update parcel delivered.
8
Enter drone id
123
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
4
Enter id of a parcel
9

Id: 9
Sender:
Id: 3
Name:
Receiver:
Id: 8
Name:
Weight: Medium
Priority: Emergency
MyDrone:
Id: 123
Battery: 58.51
CurrentPlace:
Longitude: 33.4
Latitude: 34
Requested: 02/12/2021 23:12:34
Scheduled: 02/12/2021 23:15:53
PickedUp: 02/12/2021 23:18:17
Delivered: 02/12/2021 23:18:56
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
2
Enter id of a drone
123

Id: 123
Model: bbb
Weight: Medium
Battery: 58.51
Status: Avaliable
MyParcel:
CurrentPlace:
Longitude: 33.4
Latitude: 34
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
Enter id, name, location and num of Charge slots:
444
ttt
30.1
23.4
5
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
2
Enter a number:
1- To update drone model,
2- To update a base station,
3- To update a customer,
4- To update drone to charge,
5- To update discharge drone,
6- To update parcel to drone,
7- To update parcel collect,
8- To update parcel delivered.
2
Enter id, name and num slots of a base station
444
zzz
31
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
444

Id: 444
Name: zzz
Place:
Longitude: 30.1
Latitude: 23.4
AvaliableSlots: 31
DroneSlots: System.Linq.Enumerable+WhereSelectListIterator`2[IDAL.DO.DroneCharge,IBL.BO.DroneInCharge]
The drones in charge:

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
1

Id: 1
Name:
Place:
Longitude: 29.7
Latitude: 33.9
AvaliableSlots: 1
DroneSlots: System.Linq.Enumerable+WhereSelectListIterator`2[IDAL.DO.DroneCharge,IBL.BO.DroneInCharge]
The drones in charge:
2 3
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
6- To print all avaliable base stations.
1

Id: 1
Name:
AvaliableSlots: 1
DisAvaliableSlots: 2

Id: 2
Name:
AvaliableSlots: 4
DisAvaliableSlots: 2

Id: 444
Name: zzz
AvaliableSlots: 31
DisAvaliableSlots: 0
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
6- To print all avaliable base stations.
3

Id: 1
Name:
PhoneNum:
NumArrived: 0
NumSend: 1
NumGot: 0
NumOnWay: 0

Id: 2
Name:
PhoneNum:
NumArrived: 0
NumSend: 2
NumGot: 0
NumOnWay: 0

Id: 3
Name:
PhoneNum:
NumArrived: 1
NumSend: 2
NumGot: 0
NumOnWay: 0

Id: 4
Name:
PhoneNum:
NumArrived: 0
NumSend: 0
NumGot: 0
NumOnWay: 0

Id: 5
Name:
PhoneNum:
NumArrived: 0
NumSend: 2
NumGot: 0
NumOnWay: 0

Id: 6
Name:
PhoneNum:
NumArrived: 0
NumSend: 0
NumGot: 0
NumOnWay: 0

Id: 7
Name:
PhoneNum:
NumArrived: 0
NumSend: 0
NumGot: 0
NumOnWay: 0

Id: 8
Name:
PhoneNum:
NumArrived: 0
NumSend: 0
NumGot: 1
NumOnWay: 0

Id: 9
Name:
PhoneNum:
NumArrived: 0
NumSend: 1
NumGot: 0
NumOnWay: 0

Id: 10
Name:
PhoneNum:
NumArrived: 0
NumSend: 2
NumGot: 0
NumOnWay: 0
Enter a number:
0- Exit,
1- To add,
2- To update,
3- To print,
4- To print all.
0
*/


