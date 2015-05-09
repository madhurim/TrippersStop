using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippism.Areas.IATA.Models
{
    public class Entity
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}