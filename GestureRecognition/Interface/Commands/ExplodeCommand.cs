namespace GestureRecognition.Interface.Commands
{
    public class ExplodeCommand : AInterpretedCommand
    {
        protected override string InfoDump()
        {
            return "ExplodeCommand";
        }

        public ExplodeCommand(string id, string name) : base(id, name)
        {
        }

        public ExplodeCommand()
        {}
    }
}