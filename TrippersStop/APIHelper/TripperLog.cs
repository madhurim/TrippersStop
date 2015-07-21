using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Trippism.APIHelper
{
    public class TripperLog
    {

        private static object loggingLocker = new object();
        public static bool IsMethodLoggingEnabled
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableMethodLogging"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnableMethodLogging"]);
            }
        }

        /// <summary>
        /// Write method log into file.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="milliseconds"></param>
        public static void LogMethodTime(string methodName, double milliseconds)
        {
            if (IsMethodLoggingEnabled)
            {
                WriteToLogFile(methodName + " : time taken " + milliseconds.ToString() + " : milliseconds");
            }
        }

        /// <summary>
        /// write content to file.
        /// </summary>
        /// <param name="content"></param>
        private static void WriteToLogFile(string content)
        {
            string path = string.Empty;
            StreamWriter streamWriter = null;
            lock (loggingLocker)
            {
                if (!string.IsNullOrEmpty(content))
                {
                    path = Convert.ToString(ConfigurationManager.AppSettings["LoggingFilePath"]);
                    if (!File.Exists(path))
                    {
                        // Create a file to write to. 
                        using (streamWriter = File.CreateText(path))
                        {
                            streamWriter.WriteLine(string.Format("{0} : {1}", DateTimeOffset.Now.ToString(), content));
                        }
                    }
                    else
                    {
                        using (streamWriter = File.AppendText(path))
                        {
                            streamWriter.WriteLine(string.Format("{0} : {1}", DateTimeOffset.Now.ToString(), content));
                        }
                    }
                }
            }
        }
    }
}