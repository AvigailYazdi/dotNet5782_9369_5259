using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        [Serializable]
        public class NegativeSlotsException : Exception
        {
            public int Id { get; set; }
            public NegativeSlotsException() : base() { }
            public NegativeSlotsException(int id) : base() 
            {
                Id = id;
            }
            public NegativeSlotsException(int id, string message) : base(message)
            {
                Id = id;
            }
            public NegativeSlotsException(int id, string message, Exception inner) : base(message, inner)
            {
                Id = id;
            }
            public override string ToString()
            {
                return $"The number of avaliable slots of basstation {Id} has to be positive.";
            }
        }
        [Serializable]
        public class MissingIdException : Exception
        {
            public string EntityName { get; set; }
            public int Id { get; set; }
            public MissingIdException() : base() { }
            public MissingIdException(int id, string entity) : base()
            {
                Id = id;
                EntityName = entity;
            }
            public MissingIdException(int id, string entity, string message) : base(message)
            {
                Id = id;
                EntityName = entity;
            }
            public MissingIdException(int id, string entity, string message, Exception inner) : base(message, inner)
            {
                Id = id;
                EntityName = entity;
            }
            public override string ToString()
            {
                return $"The {EntityName} id: {Id} is not exist";
            }
        }
        [Serializable]
        public class DuplicateIdException : Exception
        {
            public string EntityName { get; set; }
            public int Id { get; set; }
            public DuplicateIdException() : base() { }
            public DuplicateIdException(int id, string entity) : base()
            {
                Id = id;
                EntityName = entity;
            }
            public DuplicateIdException(int id, string entity, string message) : base(message)
            {
                Id = id;
                EntityName = entity;
            }
            public DuplicateIdException(int id, string entity, string message, Exception inner) : base(message, inner)
            {
                Id = id;
                EntityName = entity;
            }
            public override string ToString()
            {
                return $"The {EntityName} id: {Id} is already exist";
            }
        }
        [Serializable]
        public class NotEnoughBatteryException : Exception
        {
            public int Id { get; set; }
            public NotEnoughBatteryException() : base() { }
            public NotEnoughBatteryException(int id) : base()
            {
                Id = id;
            }
            public NotEnoughBatteryException(int id, string message) : base(message)
            {
                Id = id;
            }
            public NotEnoughBatteryException(int id, string message, Exception inner) : base(message, inner)
            {
                Id = id;
            }
            public override string ToString()
            {
                return $"The battery of id: {Id} drone is not enough";
            }
        }
        [Serializable]
        public class NotAvaliableDroneException : Exception
        {
            public int Id { get; set; }
            public NotAvaliableDroneException() : base() { }
            public NotAvaliableDroneException(int id) : base()
            {
                Id = id;
            }
            public NotAvaliableDroneException(int id, string message) : base(message)
            {
                Id = id;
            }
            public NotAvaliableDroneException(int id, string message, Exception inner) : base(message, inner)
            {
                Id = id;
            }
            public override string ToString()
            {
                return $"The drone: {Id} is not avaliable";
            }
        }
        [Serializable]
        public class NotMaintenanceDroneException : Exception
        {
            public int Id { get; set; }
            public NotMaintenanceDroneException() : base() { }
            public NotMaintenanceDroneException(int id) : base()
            {
                Id = id;
            }
            public NotMaintenanceDroneException(int id, string message) : base(message)
            {
                Id = id;
            }
            public NotMaintenanceDroneException(int id, string message, Exception inner) : base(message, inner)
            {
                Id = id;
            }
            public override string ToString()
            {
                return $"The drone: {Id} is not maintenance";
            }
        }
        [Serializable]
        public class NotInDeliveryException : Exception
        {
            public int Id { get; set; }
            public NotInDeliveryException() : base() { }
            public NotInDeliveryException(int id) : base()
            {
                Id = id;
            }
            public NotInDeliveryException(int id, string message) : base(message)
            {
                Id = id;
            }
            public NotInDeliveryException(int id, string message, Exception inner) : base(message, inner)
            {
                Id = id;
            }
            public override string ToString()
            {
                return $"The drone: {Id} is not in delivery";
            }
        }
        [Serializable]
        public class NotAvaliableStationException : Exception
        {
            public NotAvaliableStationException() : base() { }
            public NotAvaliableStationException(string message) : base(message) { }
            public NotAvaliableStationException(string message, Exception inner) : base(message, inner) { }
            public override string ToString()
            {
                return $"There are no avaliable stations.";
            }
        }
    }
}

