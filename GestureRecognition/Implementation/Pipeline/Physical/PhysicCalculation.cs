using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GestureRecognition.Interface.Commands;
using Trame;
using Trame.Implementation.Skeleton;
using TrameSkeleton.Math;


[assembly: InternalsVisibleTo("GestureRecognitionTest")]
namespace GestureRecognition.Implementation.Pipeline.Physical
{
    /// <summary>
    /// Calculation for physical commands
    /// </summary>
    class PhysicCalculation
    {
       
        public PhysicCommand CreatePhysicCommand(IList<ISkeleton> newests)
        {
            var newestSkeleton = newests.First();
            var prevSkeleton = newests.Count > 1 ? newests[1] : null;
            var beforePrevSkeleton = newests.Count > 2 ? newests[2] : null;
            var command = new PhysicCommand { UserId = newests.First().ID };
            uint time1 = 1, time2 = 1;

            if (prevSkeleton != null)
            {
                time2 = newestSkeleton.Timestamp - prevSkeleton.Timestamp;
                if (beforePrevSkeleton != null)
                {
                    time1 = prevSkeleton.Timestamp - beforePrevSkeleton.Timestamp;
                }

            }
            
            foreach (var part in newestSkeleton.Joints.Where(Filter).Select(ExtractData))
            {
                if (prevSkeleton != null)
                {
                    var prevJoint = prevSkeleton.GetJoint((JointType) part.Id);
                    part.Velocity = CalculateVelocity(prevJoint.Point, part.Position, time2);
                    if (beforePrevSkeleton != null)
                    {
                        var beforePrevJoint = beforePrevSkeleton.GetJoint((JointType)part.Id);
                        part.Acceleration = CalculateAcceleration(beforePrevJoint.Point, prevJoint.Point, part.Position, time1, time2);
                    }
                }
                
                command.AddCollider(part);
            }

            return command;
        }

        private bool Filter(IJoint joint)
        {
            var allowedJoints = new List<JointType> { JointType.ELBOW_LEFT, JointType.ELBOW_RIGHT, JointType.WRIST_LEFT, JointType.WRIST_RIGHT, JointType.HAND_LEFT, JointType.HAND_RIGHT };
            return allowedJoints.Contains(joint.JointType);
        }

        private BodyPart ExtractData(IJoint joint)
        {
            var rotation = joint.Orientation;
            var worldCoordinate = joint.Point;
            var part = new BodyPart
            {
                Id = joint.JointType.ToInt(),
                Position = worldCoordinate,
                Rotation = rotation,
                Length = Creator.GetDefaultBoneLength(joint.JointType),
                Velocity = new Vector3(),
                Acceleration = new Vector3(),
                AngularVelocity = new Vector3(),
                AngularAcceleration = new Vector3()
            };
            return part;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Vector3 CalculateVelocity(Vector3 p1, Vector3 p2, long time)
        {
            return (p2 - p1)/time;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static Vector3 CalculateAcceleration(Vector3 p1, Vector3 p2, Vector3 p3, long time1, long time2)
        {
            return (CalculateVelocity(p2, p3, time2) - CalculateVelocity(p1, p2, time1)) / time2;
        }
    }
}
