namespace GestureRecognition.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;

    using global::GestureRecognition.Implementation.Serializer;
    using global::GestureRecognition.Interface;

    using Leap;

    public class LeapPlayer : ILeapController
    {
        private const int NextFrameSizeLength = 4;

        /// <summary>
        /// True if the output will be looped after reaching the end of the file, otherwise false.
        /// </summary>
        public bool LoopOutput = false;

        private CancellationTokenSource _cancellationTokenSource;

        private string _path;

        private System.Threading.Tasks.Task _producerTask;

        private System.Threading.Tasks.Task _consumerTask;

        private long numberOfFrames = -1;

        private ConcurrentQueue<Frame> readyFrames = new ConcurrentQueue<Frame>();

        public LeapPlayer(string path)
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._path = path;
            
            this._producerTask = new System.Threading.Tasks.Task(this.FillFrameBuffer, this._cancellationTokenSource.Token);
            this._consumerTask = new System.Threading.Tasks.Task(this.ConsumeFrameBuffer, this._cancellationTokenSource.Token);
        }

        public long TotalBytesRead { get; private set; } = 0;

        public event Action<Frame> FrameReady;

        /// <summary>
        /// Gets the number of bytes available in the underlying file of this player.
        /// </summary>
        public long FileSizeInBytes
        {
            get
            {
                if (this.numberOfFrames == -1)
                {
                    FileInfo loopbackFileInfo = new FileInfo(this._path);
                    this.numberOfFrames = loopbackFileInfo.Length;
                }

                return this.numberOfFrames;
            }
        }

        public void StartConnection()
        {
            this._producerTask.Start();
            this._consumerTask.Start();
        }

        public void StopConnection()
        {
            this._cancellationTokenSource.Cancel();
        }

        private void ConsumeFrameBuffer()
        {
            while (!this._cancellationTokenSource.Token.IsCancellationRequested)
            {
                Frame nextFrame;
                bool frameSuccessfullyDequeued = this.readyFrames.TryDequeue(out nextFrame);

                if (frameSuccessfullyDequeued)
                {
                    this.FireFrameReady(nextFrame);
                }
            }
        }

        private void FillFrameBuffer()
        {
            // load file
            var sizeBytes = new byte[NextFrameSizeLength];
            var serializer = new LeapFrameSerializer();
            using (var file = File.Open(this._path, FileMode.Open))
            {
                while (!this._cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // Read how long the next frame is.
                    var bytesRead = file.Read(sizeBytes, 0, NextFrameSizeLength);
                    this.TotalBytesRead += bytesRead;

                    // If the bytes read are less than 4 bytes, the end of the file has been reached. Otherwise we have more data to read.
                    if (bytesRead == NextFrameSizeLength)
                    {
                        // Process the playback data read from the file.
                        var length = BitConverter.ToInt32(sizeBytes, 0);
                        this.TotalBytesRead += length;
                        var objBuffer = new byte[length];
                        file.Read(objBuffer, 0, length);
                        var frame = serializer.Deserialize(objBuffer);

                        this.readyFrames.Enqueue(frame);
                    }
                    else if (this.LoopOutput)
                    {
                        // If the output should loop, reset the stream when the end of the file is reached.
                        file.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
        }

        private void FireFrameReady(Frame f)
        {
            this.FrameReady?.Invoke(f);
        }
    }
}