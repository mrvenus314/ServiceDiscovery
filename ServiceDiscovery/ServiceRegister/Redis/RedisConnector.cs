using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRegister.Redis
{
    /// <summary>
    /// Redis连接器
    /// </summary>
    public static class RedisConnector
    {
        /// <summary>
        /// 获取Redis数据库
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IDatabase GetDatabase(string host, string port)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"{host}:{port}");

            var db = redis.GetDatabase();
            return db;
        }
    }
}
