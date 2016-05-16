using NLog;
using System.Threading.Tasks;

namespace TrippismProfiles
{
    /// <summary>
    /// This class is used to save log information
    /// </summary>
    public class TrippismNLog
    {
        /// <summary>
        /// This method is used to save log information
        /// </summary>
        public async static void SaveNLogData(string logData, string loggerName = "TraceLogger")
        {
            Logger logger = LogManager.GetLogger(loggerName);
            await Task.Run(() => { logger.Trace(logData); });
        }
    }
}