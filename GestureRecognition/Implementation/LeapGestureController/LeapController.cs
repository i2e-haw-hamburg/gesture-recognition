using System;
using GestureRecognition.Interface;
using Leap;

namespace GestureRecognition.Implementation
{
    public class LeapController : ILeapController
    {
        public event Action<Frame> FrameReady;
        private Controller _controller;
        public void StartConnection()
        {
            _controller = new Controller();
            _controller.FrameReady += OnNewFrame;
            _controller.StartConnection();
        }

        private void OnNewFrame(object sender, FrameEventArgs e)
        {
            FrameReady?.Invoke(e.frame);
        }

        public void StopConnection()
        {
            _controller.StopConnection();
            _controller.FrameReady -= OnNewFrame;

        }
    }
}