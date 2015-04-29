using System;
using System.Collections.Generic;

namespace Damascus.Core.Serialization
{
    //This is a quick and dirty one. Need to expire items.
    public class MemorySerializer : IDataSerializer
    {
        public IDictionary<string, DataStorage> ActiveWorkflows { get; set; }

        public MemorySerializer()
        {
            ActiveWorkflows = new Dictionary<string, DataStorage>();
        }

        public void SerializeData(string key, DataStorage data)
        {
            ActiveWorkflows[key] = data;
        }

        public DataStorage DeserializeData(string key)
        {
            DataStorage storedData;
            return ActiveWorkflows.TryGetValue(key, out storedData) ? storedData : null;
        }
    }
}
