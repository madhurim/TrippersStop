using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ICustomerActivity
    {
        ObjectId _id { get; set; }
        Guid CustomerGuid { get; set; }
        int ActivityTypeID { get; set; }
        DateTime ActivityDate { get; set; }
        //enum ActivityType { }
    }
}
