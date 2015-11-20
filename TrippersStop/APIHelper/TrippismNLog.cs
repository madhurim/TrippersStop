using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Trippism.APIHelper
{
    public class TrippismNLog
    {
        public async static void SaveNLogData(string logData)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            //Logger logger = LogManager.GetLogger("dbLogger");
            await Task.Run(() =>
            { logger.Trace(logData); });
        }
    }
}