using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using GestureRecognition.Utility;
using Leap;
using Trame;

namespace GestureRecognition.Implementation.Task
{
    public class MotionRecognitionTask
    {
        private RecognitionState _state = RecognitionState.Start;
        private Frame _startFrame = null;


        public void Do(BlockingCollection<Frame> frameBuffer, Action<IEnumerable<Result>> fireNewMotions, CancellationToken token)
        {
            var data = new Dictionary<JointType, InputVector>();
            foreach (var frame in frameBuffer.GetConsumingEnumerable())
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                var hands = frame.Hands;
                if (hands.Count == 0)
                {
                    _state = RecognitionState.Start;
                    _startFrame = null;
                    continue;
                }
                CheckSingleHandGestures(frame, fireNewMotions);
                var leftHand = frame.Hands.Find(h => h.IsLeft);
                var rightHand = frame.Hands.Find(h => h.IsRight);
                if (!BothHandsInFrame(leftHand, rightHand))
                {
                    _state = RecognitionState.Start;
                    _startFrame = null;
                    continue;
                }
                if (_state == RecognitionState.Start && IsStartPose(leftHand, rightHand))
                {
                    _startFrame = frame;
                    _state = RecognitionState.StartDetected;
                    continue;
                }
                if(_state == RecognitionState.StartDetected && CheckTwoHandGestures(leftHand, rightHand, fireNewMotions))
                {
                    _startFrame = frame;
                    _state = RecognitionState.Start;
                }
            }
        }

        private static bool BothHandsInFrame(Hand leftHand, Hand rightHand)
        {
            return leftHand != null && rightHand != null;
        }

        private bool CheckTwoHandGestures(Hand leftHand, Hand rightHand, Action<IEnumerable<Result>> fireNewMotions)
        {
            var leftHandOld = _startFrame.Hands.Find(h => h.IsLeft);
            var rightHandOld = _startFrame.Hands.Find(h => h.IsRight);
            var rightToLeftOld = leftHandOld.PalmPosition - rightHandOld.PalmPosition;
            var rightToLeft = leftHand.PalmPosition - rightHand.PalmPosition;
            var center = rightHandOld.StabilizedPalmPosition +
                         (rightToLeftOld) /2;
            var cmd = new ScaleAndRotate
            {
                Scale = (leftHand.PalmPosition - rightHand.PalmPosition).MagnitudeSquared / (rightToLeftOld).MagnitudeSquared,
                Center = center,
                Rotation = new Vector(rightToLeft.Pitch - rightToLeftOld.Pitch, rightToLeft.Yaw - rightToLeftOld.Yaw, rightToLeft.Roll - rightToLeftOld.Roll)
            };
            var result = new Result(cmd, 0.9);
            fireNewMotions(new List<Result> {result});
            return true;
        }

        private static void CheckSingleHandGestures(Frame frame, Action<IEnumerable<Result>> fireNewMotions)
        {
            var grabbedHands = frame.Hands.Where(hand => hand.GrabStrength > 0.7);
            var pinchedHands = frame.Hands.Where(hand => hand.PinchStrength > 0.7);
            var results = grabbedHands.Select(hand => new Result(new GrabCommand(hand.IsLeft)
            {
                Position = hand.PalmPosition,
                Normal = hand.PalmNormal
            }, hand.GrabStrength));
            fireNewMotions(results.Concat(pinchedHands.Select(hand => new Result(new GrabCommand(hand.IsLeft)
            {
                Position = hand.PalmPosition,
                Normal = hand.PalmNormal
            }, hand.PinchStrength))));
        }
        
        /// <summary>
        /// Start pose is detected, when 
        /// </summary>
        /// <param name="leftHand"></param>
        /// <param name="rightHand"></param>
        /// <returns></returns>
        private static bool IsStartPose(Hand leftHand, Hand rightHand)
        {
            var grabTheshold = 0.3;
            if (leftHand.GrabStrength > grabTheshold || rightHand.GrabStrength > grabTheshold)
            {
                return false;
            }
            return Geometry.DirectTo(leftHand.PalmPosition, leftHand.PalmNormal, rightHand.PalmPosition) &&
                   Geometry.DirectTo(rightHand.PalmPosition, rightHand.PalmNormal, leftHand.PalmPosition);
        }
    }

    internal enum RecognitionState
    {
        Start,
        StartDetected
    }
}