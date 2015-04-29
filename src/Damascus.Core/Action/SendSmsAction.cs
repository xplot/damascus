using Damascus.Message;

namespace Damascus.Core
{
    public abstract class SendSmsAction : IStepAction
    {
        private string _phone, _message;

        public SendSmsAction(string phone, string message)
        {
            this._message = message;
            this._phone = phone;
        }

        public abstract IActionOutput Execute(IStepInput input);
    }
}