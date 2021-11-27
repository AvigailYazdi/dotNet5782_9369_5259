using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
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
                return base.ToString() + $" ,The {EntityName} id: {Id} is not exist";
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
                return base.ToString() + $" ,The {EntityName} id: {Id} is already exist";
            }
        }
    }
}
