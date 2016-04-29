
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace TrippismEntities
{
    /// <summary>
    /// A non-instantiable base entity which defines 
    /// members available across all entities.
    /// </summary>
    public abstract class EntityBase
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }

        //TBD : Need to check
       
        //[BsonId]
        //public ObjectId _id { get; set; }
    } 
}
