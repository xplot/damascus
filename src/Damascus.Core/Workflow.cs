using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Castle.Windsor;
using Damascus.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Damascus.Core
{
    public abstract class Workflow : IWorkflow
    {
        private string _id;
        protected Workflow()
        {
            this.Data = new DataStorage();
        }
        
        public string Id {
            get { return _id; }
            set
            {
                _id = value;
                if(!string.IsNullOrEmpty(_id))
                    Data["id"] = _id;
            }
        }

        public Dictionary<string,IStep> Steps { get; set; }
        
        public DataStorage Data { get; set; }

        public IDataSerializer DataSerializer { get; set; }

        public virtual string Execute(string step, IStepInput input)
        {
            if (!Steps.ContainsKey(step))
                throw new Exception("There is no step defined for workflow: " + this.GetType().Name + " with name: " + step );

            return Steps[step].Execute(input);
        }
    }
}
