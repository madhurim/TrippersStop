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
        public double RedisExpireInMin 
        {
            get 
            {
                return double.Parse(WebConfigurationManager.AppSettings["RedisExpireInMin"].ToString()); 
            }
        }
        public string RedisHost
        {
            get
            {
                return WebConfigurationManager.AppSettings["RedisServer"];
            }
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

    } 

}
