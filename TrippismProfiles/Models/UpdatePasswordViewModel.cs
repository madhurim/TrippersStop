using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrippismProfiles
{
    public class UpdatePasswordViewModel
    {
        [Required]

        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
        public Guid CustomerGuid { get; set; }
    }
}