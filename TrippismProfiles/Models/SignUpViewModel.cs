using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrippismProfiles.Models
{
    public class SignUpViewModel
    {
        //[BsonId]
        //public ObjectId _id { get; set; }
        public Guid Id { get; set; }
        //public Guid Id { get; set; } 
       // public string SessionTokenID { get; set; }
        public int AuthenticationType { get; set; }
        public Guid CustomerGuid { get; set; }
        [Required]
        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEmailVerified { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}