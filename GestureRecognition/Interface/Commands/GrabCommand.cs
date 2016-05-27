namespace GestureRecognition.Interface.Commands
{
    public class GrabCommand : AInterpretedCommand
    {
        protected override string InfoDump()
        {
            return "GrabCommand";
        }

        public GrabCommand(string id, string name) : base(id, name)
        {
        }

        public GrabCommand()
        {
            this.Id = "GrabCommand";
            this.Name = "GrabCommand";
        }
    }
}