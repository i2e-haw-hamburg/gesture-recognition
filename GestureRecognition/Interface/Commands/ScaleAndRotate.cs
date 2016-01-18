namespace GestureRecognition.Interface.Commands
{
    public class ScaleAndRotate : AInterpretedCommand
    {
        protected override string InfoDump()
        {
            return "ScaleAndRotate";
        }

        public ScaleAndRotate(string id, string name) : base(id, name)
        {
        }

        public ScaleAndRotate()
        {}
    }
}