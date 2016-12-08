using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using StackExchange.Redis;
using System.Linq.Expressions;
using System.Reflection;
//using TraveLayer.APIServices;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Configuration;
using System.Web.Configuration;
using EmailService;

namespace TrippismApi.TraveLayer
{

    public class RedisService : ICacheService
    {
        String _RedisServer;
        double _RedisExpireInMin;
        String _RedisSlave;
        String _RedisPassword;
        int _RedisPort;

        public double RedisExpireInMin
        {
            get
            {
                return _RedisExpireInMin;
            }
            set
            {
                _RedisExpireInMin = value;
            }
        }
        public string RedisHost
        {
            get
            {
                return _RedisServer;
            }
            set
            {
                _RedisServer = value;
            }
        }

        public RedisService()
        {
            _RedisServer = ConfigurationManager.AppSettings["RedisServerMaster"].ToString();
            _RedisExpireInMin = double.Parse(ConfigurationManager.AppSettings["RedisExpireInMin"].ToString());
            _RedisPassword = ConfigurationManager.AppSettings["RedisPassword"].ToString();
            _RedisPort = int.Parse(ConfigurationManager.AppSettings["RedisPort"].ToString());
        }

        public bool Save<T>(string key, T keyData, double expireInMin)
        {

            bool isSuccess = false;
            try
            {

                using (var redisClient = new RedisClient(RedisHost, _RedisPort, _RedisPassword))
                {
                    isSuccess = redisClient.Set<T>(key, keyData, DateTime.Now.AddMinutes(expireInMin));
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public bool Save<T>(string key, T keyData)
        {
            bool isSuccess = false;
            try
            {
                using (var redisClient = new RedisClient(RedisHost, _RedisPort, _RedisPassword))
                {
                    isSuccess = redisClient.Set<T>(key, keyData, DateTime.Now.AddMinutes(RedisExpireInMin));
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public bool Expire(string key)
        {
            bool isSuccess = false;
            try
            {
                using (var redisClient = new RedisClient(RedisHost, _RedisPort, _RedisPassword))
                {
                    isSuccess = redisClient.Expire(key, 0);
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        public T GetByKey<T>(string key)
        {
            try
            {
                //  using (var redisClient = new RedisClient(RedisHost))
                //  {
                //check if Master is available
                var redisClient = new RedisClient(RedisHost, _RedisPort, _RedisPassword);
                if (IsConnected())
                {
                    return redisClient.Get<T>(key);
                }
                else
                {
                    _RedisSlave = ConfigurationManager.AppSettings["RedisServerSlave"].ToString();
                    redisClient.Dispose();
                    using (var redisClientSlave = new RedisClient(_RedisSlave))
                    {
                        return redisClientSlave.Get<T>(key);
                    }
                }
                //  }
            }
            catch
            {
                return default(T);
            }
        }

        public bool IsConnected()
        {
            bool isSuccess = false;
            try
            {
                using (var redisClient = new RedisClient(RedisHost, _RedisPort, _RedisPassword))
                {
                    isSuccess = redisClient.Ping();
                }
                if (!isSuccess)
                    connectionFailMail("");
            }
            catch (Exception ex)
            {
                isSuccess = false;
                connectionFailMail(ex.Message.ToString());
            }
            return isSuccess;
        }
        public void connectionFailMail(string ErrorMessage)
        {
            List<string> listToaddress = new List<string>();
            listToaddress.Add("subham@trivenitechnologies.in");

            //---------------- Start Send Redis Connection Failed to selected Member ----------------------
            //Disable Getting Config Email Address Due to still not set up Redis on server

            //string email = ConfigurationManager.AppSettings["ConnectionFailNotification"].ToString();

            //var toemail = email.Split(',');
            //List<string> listToaddress = new List<string>();
            //foreach (var toaddress in toemail)
            //    listToaddress.Add(toaddress);

            //----------------End Send Redis Connection Failed to selected Member ----------------------


            string fromEmail = ConfigurationManager.AppSettings["MailGunFromemail"];

            // mail.SendComplexMessage("noreply@trippism.com", "Redis connection failed", listToaddress, "<html><body><div><p><strong>Title: </strong>Redis connection failed.</p><p><strong>Time: </strong>" + DateTime.Now.ToString() + "</p><p><strong style='color:#b90005;'>Error Message: </strong>" + ErrorMessage + "</p><p></p></div></body></html>");

            IEmailService iemail = new SESEmail();
            iemail.SendMessage(fromEmail, "Redis connection failed", listToaddress, "<html><body><div><p><strong>Title: </strong>Redis connection failed.</p><p><strong>Time: </strong>" + DateTime.Now.ToString() + "</p><p><strong style='color:#b90005;'>Error Message: </strong>" + ErrorMessage + "</p><p></p></div></body></html>");

        }

    }

}
