namespace Damascus.Core
{
    public interface IActionOutput
    {
        string Output { get; set; }
    }
    public class ActionOutput : IActionOutput
    {
        public string Output { get; set; }
    }
}