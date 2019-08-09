using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace ConsoleApp1
{
    /// <summary>
    /// 通过redis来存储红包
    /// </summary>
    public class RedisRedPackageRepository : IRedPackageRepository
    {
        private static object lockObj = new object();
        private static ConnectionMultiplexer _redis;
        private static ConnectionMultiplexer redis
        {
            get
            {
                if (_redis == null)
                {
                    lock (lockObj)
                    {
                     
                        _redis = ConnectionMultiplexer.Connect("localhost:6379");
                        return _redis;
                    }
                }
                return _redis;
            }
        }

        public double Get(string redId)
        {
            return (double)redis.GetDatabase(0).ListRightPop(redId);
        }

        public void Save(string redId, List<double> list)
        {
            foreach (var item in list)
            {
                redis.GetDatabase(0).ListRightPush(redId, (RedisValue)item);
            }
        }
    }
}
