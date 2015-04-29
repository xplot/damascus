using System.Collections.Generic;

namespace Damascus.Core
{
    public class DataStorage:Dictionary<string,string>
    {
        public DataStorage()
        {
        }

        public DataStorage(Dictionary<string, string> input):base(input)
        {
        }

        public void Update(Dictionary<string, string> input)
        {
            foreach (string key in input.Keys)
            {
                this[key] = input[key];
            }
        }

        public new string this[string key]
        {
            get
            {
                return !ContainsKey(key) ? null : base[key];
            }
            set { base[key] = value; }
        }
    }

    public interface IDataSerializer
    {
        void SerializeData(string key, DataStorage data);

        DataStorage DeserializeData(string key);
    }
}