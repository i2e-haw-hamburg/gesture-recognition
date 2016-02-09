namespace GestureRecognition.Interface.Commands
{
    public class NoneCommand : AInterpretedCommand
    {
        protected override string InfoDump()
        {
            return "NoneCommand";
        }

        public NoneCommand(string id, string name) : base(id, name)
        {
        }

        public NoneCommand()
        {
            this.Id = "NoneCommand";
            this.Name = "NoneCommand";
        }
    }
}