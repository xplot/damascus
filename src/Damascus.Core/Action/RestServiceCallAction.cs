using System;
using Damascus.Message;

namespace Damascus.Core
{
    public class RestServiceCallAction : IStepAction
    {
        private string _url, _method;

        public RestServiceCallAction(string url, string method)
        {
            this._url = url;
            this._method = method;
        }

        public IActionOutput Execute(IStepInput input)
        {
            throw new NotImplementedException();
        }
    }
}