using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Message
{
    public interface IStep
    {
        string Execute(IStepInput input);
    }

    public interface IStepInput
    {
        string this[string key] { get; set; }
        IDictionary<string, string> Input { get; set; }
    }

    public class StepInput : IStepInput
    {
        public IDictionary<string, string> Input { get; set; }

        public string this[string key]
        {
            get
            {
                if (this.Input.ContainsKey(key))
                    return this.Input[key];
                return null;
            }
            set
            {
                this.Input[key] = value;
            }
        }

        public bool ContainsKey(string key)
        {
            return this.Input.ContainsKey(key);
        }
    }
}
