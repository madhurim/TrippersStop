//using ServiceStack.Redis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TrippersStop.TraveLayer
//{
//    class RedisConnector
//    {
//        private static RedisConnector _instance;
//        private static RedisClient _client { get; set; }
//        private static double expireTime = 10;
//        private static string redisHost = "127.0.0.1:6379";
//        // Lock synchronization object
//        private static object syncLock = new object();
//        // Constructor (protected)
//        protected RedisConnector()
//        {
//            _client = new RedisClient(redisHost);
//        }
//        public static RedisConnector GetRedisConnector()
//        {
//            // Support multithreaded applications through
//            // 'Double checked locking' pattern which (once
//            // the instance exists) avoids locking each
//            // time the method is invoked

//            if (_instance == null)
//            {
//                lock (syncLock)
//                {
//                    if (_instance == null)
//                    {
//                        _instance = new RedisConnector();
//                    }
//                }
//            }
//            return _instance;
//        }
//        public static bool Save<T>(string key,T keyData)
//        {
//            bool isSuccess = false;
//            try
//            {
//                isSuccess = _client.Set<T>(key, keyData, DateTime.Now.AddMinutes(expireTime));
//            }
//            catch
//            {
//                isSuccess = false;
//            }
//            return isSuccess;
//        }
//        public static T GetByKey<T>(string key)
//        {
//            return _client.Get<T>(key);
//        }
//    }
//}
