using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using System.Configuration;


namespace TrippismProfiles
{
    /// <summary>
    /// It's used to set Service Stack Formatter.
    /// </summary>
    public class ServiceStackFormatterConfigAttribute : Attribute, IControllerConfiguration
    {
        /// <summary>
        /// It's used to enable ServiceStackFormatter.
        /// </summary>
        public static bool IsServiceStackFormatterEnabled
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableServiceStackFormatter"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnableServiceStackFormatter"]);
            }
        }
        /// <summary>
        /// It's used to Initialize ServiceStackFormatter.
        /// </summary>
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            if (IsServiceStackFormatterEnabled)
            {
                if (controllerSettings.Formatters.Count>0)
                controllerSettings.Formatters.RemoveAt(0);
                controllerSettings.Formatters.Insert(0, new ServiceStackTextFormatter());
            }
        }
    }
}