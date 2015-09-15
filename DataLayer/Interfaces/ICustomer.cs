using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    interface ICustomer
    {
        ObjectId _id { get; set; }
        Guid CustomerGuid { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DOB { get; set; }
        string Gender { get; set; }
        string Mobile { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }


    }
}
