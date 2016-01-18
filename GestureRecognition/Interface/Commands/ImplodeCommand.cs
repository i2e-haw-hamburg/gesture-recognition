namespace GestureRecognition.Interface.Commands
{
    public class ImplodeCommand : AInterpretedCommand
    {
        protected override string InfoDump()
        {
            return "ImplodeCommand";
        }

        public ImplodeCommand(string id, string name) : base(id, name)
        {
        }

        public ImplodeCommand()
        {}
    }
}