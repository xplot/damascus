//namespace Damascus.Core
//{
//    public class PhoneCallEnterNumberAction : IStepAction
//    {
//        private IIvrXmlWriter _xmlWriter;
//        private int _number;
//        public PhoneCallEnterNumberAction(IIvrXmlWriter xmlWriter, int number)
//        {
//            this._xmlWriter = xmlWriter;
//            this._number = number;
//        }

//        public IActionOutput Execute(IStepInput input)
//        {
//            var actionOutput = new ActionOutput()
//            {
//                this._xmlWriter.EnterNumber("Please dial in your keypad: " + this._number.ToString())
//            };

//            return actionOutput;
//        }
//    }
//}