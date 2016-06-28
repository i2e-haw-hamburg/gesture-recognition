using System;
using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using Leap;
using Trame;

namespace GestureRecognition.Interface
{
    public interface IController
    {
        event Action<AUserCommand> NewPhysicsCommand;
        event Action<IEnumerable<Result>> NewMotions;
        void PushNewSkeleton(ISkeleton skeleton);

        void Stop();
        event Action<Frame> NewFrame;
    }
}