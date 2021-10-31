using System;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// A struct of the drones.
        /// fields: id, model, max weight, status and   percent of battery.
        /// functions: To string- a function that returns the string to print.
        /// </summary>
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                return $"Drone- Id: {Id}, Model: {Model}, Max Weight: {MaxWeight}, Status: {Status}, Battery: {Battery}";
            }
        }
    }
 }
