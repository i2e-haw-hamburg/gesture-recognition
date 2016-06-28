using Leap;
using TrameSkeleton.Math;

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
        /// <summary>
        /// The position of the hand palm of the hand which performs the grab command.
        /// </summary>
        public Vector Position { get; set; }
        /// <summary>
        /// The normal of the hand palm of the hand which performs the grab command.
        /// </summary>
        public Vector Normal { get; set; }

        public Quaternion Rotation { get; set; }
    }
}