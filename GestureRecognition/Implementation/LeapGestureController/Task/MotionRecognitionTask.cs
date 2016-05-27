using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using Leap;
using Trame;

namespace GestureRecognition.Implementation.Task
{
    public class MotionRecognitionTask
    {
        public void Do(BlockingCollection<Frame> frameBuffer, Action<IEnumerable<Result>> fireNewMotions)
        {
            var data = new Dictionary<JointType, InputVector>();
            foreach (var frame in frameBuffer.GetConsumingEnumerable())
            {
                var grabbedHands = frame.Hands.Where(hand => hand.GrabStrength > 0.5);
                // fireNewMotions(grabbedHands.Select(hand => new Result(new GrabCommand())));
            }
        }
    }
}