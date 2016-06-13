using System;
using System.IO;
using System.Threading;
using GestureRecognition.Implementation.Serializer;
using GestureRecognition.Interface;
using Leap;

namespace GestureRecognition.Implementation
{

    public class LeapPlayer : ILeapController
    {
        private string _path;
        private Thread _thread;
        private CancellationTokenSource _cancellationTokenSource;

        public LeapPlayer(string path)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _path = path;
            _thread = new Thread(Run);
        }

        public void StopConnection()
        {
            _cancellationTokenSource.Cancel();
        }

        public event Action<Frame> FrameReady;

        public void StartConnection()
        {
            _thread.Start();
        }

        private void Run()
        {
            // load file
            var sizeBytes = new byte[4];
            var serializer = new LeapFrameSerializer();
            using (var file = File.Open(_path, FileMode.Open))
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    file.Read(sizeBytes, 0, 4);
                    var length = BitConverter.ToInt32(sizeBytes, 0);
                    var objBuffer = new byte[length];
                    file.Read(objBuffer, 0, length);
                    var frame = serializer.Deserialize(objBuffer);
                    FireFrameReady(frame);
                }
            }
        }

        private void FireFrameReady(Frame f)
        {
            FrameReady?.Invoke(f);
        }
    }
}