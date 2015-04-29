using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Core
{
    public static class Dict
    {
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>
            (this Dictionary<TKey, TValue> dictA, Dictionary<TKey, TValue> dictB) where TValue : class
        {
            //Silly Merge, but known to work, keys from dict2 commands.

            if (dictA == null)
                return dictB;
            if (dictB == null)
                return dictA;
            var result = new Dictionary<TKey, TValue>();

            foreach (var key in dictA)
            {
                result[key.Key] = key.Value;
            }

            foreach (var key2 in dictB)
            {
                result[key2.Key] = key2.Value;
            }

            return result;
        }
    }
    
}
