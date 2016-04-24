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

namespace TrippismApi.TraveLayer
{

    public class RedisService : ICacheService
    {
        String _RedisServer ;
        double _RedisExpireInMin;

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
            _RedisServer = ConfigurationManager.AppSettings["RedisServer"].ToString();
            _RedisExpireInMin = double.Parse(ConfigurationManager.AppSettings["RedisExpireInMin"].ToString());
        }

        public bool Save<T>(string key, T keyData, double expireInMin)
        {
            bool isSuccess = false;
            try
            {
                using (var redisClient = new RedisClient(RedisHost))
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

        public  bool Save<T>(string key, T keyData)
        {
            bool isSuccess = false;
            try
            {
                using (var redisClient = new RedisClient(RedisHost))
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
                using (var redisClient = new RedisClient(RedisHost))
                {
                    isSuccess = redisClient.Expire(key,0);
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        public  T GetByKey<T>(string key)
        {
            try
            {
                using (var redisClient = new RedisClient(RedisHost))
                {
                    return redisClient.Get<T>(key);
                }
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
                using (var redisClient = new RedisClient(RedisHost))
                {

                    isSuccess = redisClient.Ping();
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

    } 

}
