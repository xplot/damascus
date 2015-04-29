using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damascus.Core;
using Damascus.Message;

namespace Damascus.Web
{
    public interface IStepQuery
    {
        string GetStepFromInput(IStepInput input);
        
        string GetStepFromQueryString(IStepInput input);
    }
}
