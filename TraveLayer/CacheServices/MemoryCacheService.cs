using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using TrippismApi.TraveLayer;

namespace TraveLayer.CacheServices
{

    public class MemoryCacheService : ICacheService
    {
        public T GetByKey<T>(string key)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;
                object item = memoryCache.Get(key);
                if (item is T)
                {
                    return (T)item;
                }
            }
            catch
            {
                return default(T);
            }
            return default(T);
        }

        public double MemoryExpireInMin
        {
            get
            {
                return double.Parse(WebConfigurationManager.AppSettings["RedisExpireInMin"].ToString());
            }
        }
        public MemoryCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }
        public bool Save<T>(string key, T keyData, double expireInMin)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = Cache.Add(key, keyData, DateTimeOffset.Now.AddMinutes(expireInMin));               
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
                isSuccess = Cache.Add(key, keyData, DateTimeOffset.Now.AddMinutes(MemoryExpireInMin));               
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
               // MemoryCache memoryCache = MemoryCache.Default;
                if (Cache.Contains(key))
                {
                    Cache.Remove(key);
                    isSuccess= true;
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
