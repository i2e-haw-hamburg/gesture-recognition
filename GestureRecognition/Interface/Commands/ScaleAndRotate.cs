using TrameSkeleton.Math;

namespace GestureRecognition.Interface.Commands
{
    public class ScaleAndRotate : AInterpretedCommand
    {
        public ScaleAndRotate(string id, string name) : base(id, name)
        {
        }

        public ScaleAndRotate()
        {
            this.Id = "ScaleAndRotate";
            this.Name = "ScaleAndRotate";
        }

        public Vector3 Rotation { get; set; }
        public double Scale { get; set; }
    }
}