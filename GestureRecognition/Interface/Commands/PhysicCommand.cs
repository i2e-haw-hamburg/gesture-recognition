using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrameSkeleton.Math;

namespace GestureRecognition.Interface.Commands
{
	/// <summary>
	/// Physic Command.
	/// </summary>
    public class PhysicCommand : AUserCommand
	{
	    public readonly IList<BodyPart> BodyParts;

	    public PhysicCommand()
	    {
            BodyParts = new List<BodyPart>();
	    }

	    public uint UserId { get; set; }

	    public void AddCollider(BodyPart part)
	    {
            BodyParts.Add(part);
	    }

	    protected override string InfoDump()
	    {
	        return "Physic Command";
	    }
	}

    public class BodyPart
    {
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public Vector4 Rotation { get; set; }
        public Vector3 Velocity { get; set; }
        public float Length { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 AngularVelocity { get; set; }
        public Vector3 AngularAcceleration { get; set; }
    }
}
