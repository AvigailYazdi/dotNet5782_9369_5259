using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
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
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }
        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }

    [Serializable]
    public class DuplicatePasswordException : Exception
    {
        public DuplicatePasswordException() : base() { }
        public DuplicatePasswordException(string message) : base(message) { }
        public override string ToString()
        {
            return $"The password is already exist";
        }
    }

    [Serializable]
    public class MissingPasswordException : Exception
    {
        public MissingPasswordException() : base() { }
        public MissingPasswordException(string message) : base(message) { }
        public override string ToString()
        {
            return $"The password is not exist";
        }
    }
}
