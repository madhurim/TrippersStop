using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    interface IAuthDetails
    {
        // enum AuthenticationType { }
        ObjectId _id { get; set; }
        string Password { get; set; }
        Guid CustomerGuid { get; set; }
        string Email { get; set; }
        DateTime CreatedDate { get; set; }
        //LogIn
        DateTime LoginTime { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}
