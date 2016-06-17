using Leap;
using TrameSkeleton.Math;

namespace GestureRecognition.Interface.Commands
{
    /// <summary>
    /// Scale and rotate command
    /// </summary>
    public class ScaleAndRotate : AInterpretedCommand
    {
        public ScaleAndRotate()
        {
            this.Id = "ScaleAndRotate";
            this.Name = "ScaleAndRotate";
        }
        /// <summary>
        /// The relative rotation around x, y, z axis (Pitch, Yaw, Roll).
        /// </summary>
        public Vector Rotation { get; set; }
        /// <summary>
        /// The scale as a simple double value. Independent scaling is problematic in 3d.
        /// </summary>
        public double Scale { get; set; }
        /// <summary>
        /// The center of the command at the begin of the command.
        /// </summary>
        public Vector Center { get; set; }

        public Vector LeftHand { get; set; }
        public Vector RightHand { get; set; }
    }
}