namespace GestureRecognition.Interface.Commands
{
    public class GrabCommand : AInterpretedCommand
    {
        protected override string InfoDump()
        {
            return "GrabCommand";
        }
        
        public GrabCommand(bool leftHand)
        {
            this.Id = "GrabCommand";
            this.Name = "GrabCommand";
            this.LeftHand = leftHand;
            this.RightHand = !leftHand;
        }

        public bool RightHand { get; set; }

        public bool LeftHand { get; set; }
    }
}