using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrippismProfiles.Models
{
    public class CustomerViewModel
    {
       // [BsonId]
       // public ObjectId _id { get; set; }
        public Guid Id { get; set; }
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [DataType(DataType.Text)]
        public string LastName { get; set; }
       
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}