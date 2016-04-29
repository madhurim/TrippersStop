using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippismProfiles.Constants
{
    /// <summary>
    /// This class is used for TrippismConstants
    /// </summary>
    public static class TrippismConstants
    {
        /// <summary>
        /// Constant for Record Deleted message
        /// </summary>
        public const string RecordDeleted = "Record deleted successfully";
        /// <summary>
        /// Constant for Customer Not Found
        /// </summary>
        public const string CustomerNotFound = "Customer does not exist";
        /// <summary>
        /// Constant for Customer Already Exist
        /// </summary>
        public const string CustomerAlreadyExist = "Customer already exist";
        /// <summary>
        /// Constant for Incorrect UserName Or Password
        /// </summary>
        public const string IncorrectUserNameOrPassword = "Incorrect UserName or Password";

        public const string PasswordUpdatedSuccessfully = "Password updated successfully";

        public const string IncorrectPassword = "Incorrect old password";
    }
}