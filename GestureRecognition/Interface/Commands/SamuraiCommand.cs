namespace GestureRecognition.Interface.Commands
{
    public class SamuraiCommand : AInterpretedCommand
    {

        protected override string InfoDump()
        {
            return "SamuraiCommand";
        }

        public SamuraiCommand(string id, string name) : base(id, name)
        {
        }

        public SamuraiCommand()
        {}
    }
}