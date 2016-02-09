namespace GestureRecognition.Interface.Commands
{
    public class SamuraiCommand : AInterpretedCommand
    {

        public SamuraiCommand(string id, string name) : base(id, name)
        {
        }

        public SamuraiCommand()
        {
            this.Id = "SamuraiCommand";
            this.Name = "SamuraiCommand";
        }
    }
}