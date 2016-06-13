using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using Leap;
using Trame;

namespace GestureRecognition.Implementation.Task
{
    public class MotionRecognitionTask
    {
        private RecognitionState _state = RecognitionState.Start;


        public void Do(BlockingCollection<Frame> frameBuffer, Action<IEnumerable<Result>> fireNewMotions, CancellationToken token)
        {
            var data = new Dictionary<JointType, InputVector>();
            foreach (var frame in frameBuffer.GetConsumingEnumerable())
            {
                if (!token.IsCancellationRequested)
                {
                    var hands = frame.Hands;
                    if (hands.Count == 0)
                    {
                        _state = RecognitionState.Start;
                        continue;
                    }
                    else if(hands.Count == 1)
                    {
                        _state = RecognitionState.OneHand;
                    }
                    else
                    {
                        _state = RecognitionState.TwoHand;
                    }
                    switch (_state)
                    {
                        case RecognitionState.OneHand:
                            var grabbedHands = frame.Hands.Where(hand => hand.GrabStrength > 0.7);
                            var pinchedHands = frame.Hands.Where(hand => hand.PinchStrength > 0.7);
                            var results = grabbedHands.Select(hand => new Result(new GrabCommand(hand.IsLeft), hand.GrabStrength));
                            fireNewMotions(results.Concat(pinchedHands.Select(hand => new Result(new GrabCommand(hand.IsLeft), hand.PinchStrength))));
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    internal enum RecognitionState
    {
        Start,
        OneHand,
        TwoHand
    }
}