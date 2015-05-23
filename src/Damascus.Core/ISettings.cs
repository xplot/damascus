using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Message;

namespace Damascus.Core
{
    public interface ISettings
    {
        string Get(string key);
    }
}
