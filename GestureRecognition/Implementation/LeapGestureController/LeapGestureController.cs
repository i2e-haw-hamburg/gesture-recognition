﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Implementation.Task;
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
        public event Action<AUserCommand> NewPhysicsCommand;
        public event Action<IEnumerable<Result>> NewMotions;

        private readonly BlockingCollection<Frame> _frameBuffer;

        private Leap.Controller controller;
        private Thread _thread;

        public LeapGestureController()
        {
            // buffers
            _frameBuffer = new BlockingCollection<Frame>();
            var recognitionTask = new MotionRecognitionTask();
            var f = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            // interpreted
            var recognition = f.StartNew(() => recognitionTask.Do(_frameBuffer, FireNewMotions));
            _thread = new Thread(() => { System.Threading.Tasks.Task.WaitAll(recognition); });
            _thread.Start();
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
            if (frame.Hands.Count == 2)
            {
                var leftHand = frame.Hands.Find(h => h.IsLeft);
                var rightHand = frame.Hands.Find(h => h.IsRight);
            }
        }

        /// <summary>
        /// Lefts the hand.
        /// </summary>
        /// <returns>The hand.</returns>
        /// <param name="hand">Frame.</param>
        /// <param name="jt"></param>
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
                ).SelectMany(x => x)
            );
            return parts;
        }

        private static IEnumerable<BodyPart> CreateFinger(Finger finger, JointType jt)
        {
            var bones = Enum.GetValues(typeof (Bone.BoneType)).Cast<Bone.BoneType>()
                .Select<Bone.BoneType, Bone>(finger.Bone)
                .Where(bone => bone != null);
            return bones.Select(bone => new BodyPart
            {
                Id = BoneType2JointType(jt, bone.Type).ToInt(),
                Position = new Vector3(bone.PrevJoint.x, bone.PrevJoint.y, bone.PrevJoint.z),
                Rotation = new Vector4(bone.Direction.x, bone.Direction.y, bone.Direction.z, 0),
                Length = bone.Length
            });
        }

        private static JointType BoneType2JointType(JointType fingerType, Bone.BoneType type)
        {
            int jt;
            switch (type)
            {
                case Bone.BoneType.TYPE_METACARPAL:
                    jt = 1;
                    break;
                case Bone.BoneType.TYPE_PROXIMAL:
                    jt = 2;
                    break;
                case Bone.BoneType.TYPE_INTERMEDIATE:
                    jt = 3;
                    break;
                case Bone.BoneType.TYPE_DISTAL:
                    jt = 4;
                    break;
                default:
                    return JointType.UNSPECIFIED;
            }
            jt += fingerType.ToInt();
            return (JointType) jt;
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
            if (NewPhysicsCommand != null)
            {
                NewPhysicsCommand(cmd);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContainer"></param>
        public void FireNewMotions(IEnumerable<Result> results)
        {
            NewMotions?.Invoke(results);
        }

        public void PushNewSkeleton(ISkeleton skeleton) {}
        
    }
        
}