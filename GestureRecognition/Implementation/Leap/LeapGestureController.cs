using System;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Interface.Commands;
using Leap;
using Trame;
using Trame.Implementation.Skeleton;
using TrameSkeleton.Math;
using IController = GestureRecognition.Interface.IController;

namespace GestureRecognition.Implementation
{
    public class LeapGestureController : IController
    {
        public event Action<AUserCommand> NewCommand;
        private Leap.Controller controller;

        public LeapGestureController()
        {
            controller = new Leap.Controller();
            controller.FrameReady += (sender, args) => this.NewFrameAvailable(args.frame);
        }

        private void NewFrameAvailable(Frame frame)
        {
            if (frame.Hands.Count > 0)
            {
                var physicsCmd = new PhysicCommand();
                foreach (var hand in frame.Hands)
                {
                    var parts = CreateHand(hand, hand.IsLeft ? JointType.HAND_LEFT : JointType.HAND_RIGHT);
                    foreach (var bodyPart in parts)
                    {
                        physicsCmd.AddCollider(bodyPart);
                    }
                }
                FireNewCommand(physicsCmd);
            }
        }
        
        /// <summary>
        /// Lefts the hand.
        /// </summary>
        /// <returns>The hand.</returns>
        /// <param name="leftHand">Frame.</param>
        private static IEnumerable<BodyPart> CreateHand(Hand hand, JointType jt)
        {
            var palmNormal = hand.PalmNormal;
            var handPosition = hand.PalmPosition;
            var handPart = new BodyPart
            {
                Id = jt.ToInt(),
                Rotation = new Vector4(palmNormal.x, palmNormal.y, palmNormal.z, 0),
                Position = new Vector3(handPosition.x, handPosition.y, handPosition.z),
                Velocity = new Vector3(hand.PalmVelocity.x, hand.PalmVelocity.y, hand.PalmVelocity.z)
            };
            var parts = new List<BodyPart> { handPart };
            parts.AddRange(
                hand.Fingers.Select(
                    finger => CreateFinger(finger, FingerType2JointType(finger.Type, JointType.HAND_LEFT == jt ? 1 : -1))
                )
            );
            return parts;
        }

        private static BodyPart CreateFinger(Finger finger, JointType jt)
        {
            return new BodyPart
            {
                Id = jt.ToInt(),
                Position = new Vector3(finger.TipPosition.x, finger.TipPosition.y, finger.TipPosition.z),
                Rotation = new Vector4(finger.Direction.x, finger.Direction.y, finger.Direction.z, 0),
                Velocity = new Vector3(finger.TipVelocity.x, finger.TipVelocity.y, finger.TipVelocity.z)
            };
        }

        private static JointType FingerType2JointType(Finger.FingerType type, int side)
        {
            JointType jt;
            switch (type)
            {
                case Finger.FingerType.TYPE_INDEX:
                    jt = (side == 1) ? JointType.INDEX_FINGER_LEFT : JointType.INDEX_FINGER_RIGHT;
                    break;
                case Finger.FingerType.TYPE_MIDDLE:
                    jt = (side == 1) ? JointType.MIDDLE_FINGER_LEFT : JointType.MIDDLE_FINGER_RIGHT;
                    break;
                case Finger.FingerType.TYPE_PINKY:
                    jt = (side == 1) ? JointType.LITTLE_FINGER_LEFT : JointType.LITTLE_FINGER_RIGHT;
                    break;
                case Finger.FingerType.TYPE_RING:
                    jt = (side == 1) ? JointType.RING_FINGER_LEFT : JointType.RING_FINGER_RIGHT;
                    break;
                case Finger.FingerType.TYPE_THUMB:
                    jt = (side == 1) ? JointType.THUMB_LEFT : JointType.THUMB_RIGHT;
                    break;
                default:
                    jt = JointType.UNSPECIFIED;
                    break;
            }

            return jt;
        }
        
        private void FireNewCommand(AUserCommand cmd)
        {
            if (NewCommand != null)
            {
                NewCommand(cmd);
            }
        }

        public void PushNewSkeleton(ISkeleton skeleton) {}
        
    }
        
}