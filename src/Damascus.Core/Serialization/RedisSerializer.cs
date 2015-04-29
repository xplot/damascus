using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Damascus.Core
{
    public class RedisSerializer: IDataSerializer
    {
        public IDatabase Cache { get; set; }

        public void SerializeData(string key, DataStorage data)
        {
            string json = JsonConvert.SerializeObject(data);
            Cache.StringSet(key, json);
        }

        public DataStorage DeserializeData(string key)
        {
            var redisValue = Cache.StringGet(key);

            if (!redisValue.HasValue)
                return null;

            var json = redisValue.ToString();
            return JsonConvert.DeserializeObject<DataStorage>(json);
        }
    }
}