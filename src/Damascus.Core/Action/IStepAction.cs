using Damascus.Message;

namespace Damascus.Core
{
    public interface IStepAction
    {
        IActionOutput Execute(IStepInput input);
    }
}