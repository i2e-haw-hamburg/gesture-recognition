using System;
using Leap;

namespace GestureRecognition.Interface
{
    public interface ILeapController
    {
        event Action<Frame> FrameReady;
        void StartConnection();
        void StopConnection();

    }
}