using System.Collections;
using System.Collections.Generic;
using Damascus.Message;
using Newtonsoft.Json;

namespace Damascus.Core
{
    public interface IWorkflow
    {
        string Id { get; set; }
        string Execute(string step, IStepInput input);
        DataStorage Data { get; set; }
    }
}