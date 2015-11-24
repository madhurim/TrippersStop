using NLog;
using System.Threading.Tasks;

namespace Trippism.APIHelper
{
    public class TrippismNLog
    {
        public async static void SaveNLogData(string logData, string loggerName = "TraceLogger")
        {
            //Logger logger = LogManager.GetCurrentClassLogger();
            Logger logger = LogManager.GetLogger(loggerName);
            await Task.Run(() => { logger.Trace(logData); });
        }
    }
}