using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace Damascus.Core
{
    public class RedisSerializer: IDataSerializer
    {
        public IRedisClientsManager ClientManager { get; set; }

        public void SerializeData(string key, DataStorage data)
        {
            using(var redis = ClientManager.GetClient())
            {
                string json = JsonConvert.SerializeObject(data);
                redis.SetEntry(key, json);    
            }
        }

        public DataStorage DeserializeData(string key)
        {
            using(var redis = ClientManager.GetClient())
            {
                var redisValue = redis.GetEntry(key);

                if (string.IsNullOrEmpty(redisValue))
                    return null;
                
                var json = redisValue.ToString();
                return JsonConvert.DeserializeObject<DataStorage>(json);
            }
        }
    }
}