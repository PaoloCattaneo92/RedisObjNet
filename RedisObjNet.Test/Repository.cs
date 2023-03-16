using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisObjNet.Test;

internal static class Repository
{
    private static RedisMapper? redis;
    internal static RedisMapper Redis 
    { 
        get
        {
            if(redis == null)
            {
                var db = ConnectionMultiplexer.Connect("localhost").GetDatabase();
                redis = new RedisMapper(db);
            }
            return redis;
        }
    }
}
