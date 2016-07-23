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
        /// The amount of delay in milliseconds between the playback of two frames.
        /// </summary>
        public int FrameDelay = 0;

        /// <summary>
        /// True if the output will be looped after reaching the end of the file, otherwise false.
        /// </summary>
        public bool LoopOutput = false;

        /// <summary>
        /// The number of frames that can get read in advance.
        /// </summary>
        public int MaxQueuedFrames = 100;

        private CancellationTokenSource _cancellationTokenSource;

        private System.Threading.Tasks.Task _consumerTask;

        private string _path;

        private System.Threading.Tasks.Task _producerTask;

        private Timer consumeTimer;

        private long numberOfFrames = -1;

        private BlockingCollection<Frame> readyFrames;

        private AutoResetEvent fileClosedAutoResetEvent = new AutoResetEvent(false);

        public LeapPlayer(string path)
        {
            this.readyFrames = new BlockingCollection<Frame>(new ConcurrentQueue<Frame>(), this.MaxQueuedFrames);

            this._cancellationTokenSource = new CancellationTokenSource();
            this._path = path;

            this._producerTask = new System.Threading.Tasks.Task(
                this.FillFrameBuffer, 
                this._cancellationTokenSource.Token);
            this._consumerTask = new System.Threading.Tasks.Task(
                this.ConsumeFrameBuffer, 
                this._cancellationTokenSource.Token);
        }

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

        public long TotalBytesRead { get; private set; } = 0;

        public void StartConnection()
        {
            this._producerTask.Start();

            if (this.FrameDelay > 0)
            {
                this.consumeTimer = new Timer(state => this.ConsumeFrame(), null, 0, this.FrameDelay);
            }
            else
            {
                this._consumerTask.Start();
            }
        }

        public void StopConnection()
        {
            this.consumeTimer?.Dispose();
            this._cancellationTokenSource.Cancel();

            // Wait for the file to be closed.
            this.fileClosedAutoResetEvent.WaitOne();
        }

        private void ConsumeFrame()
        {
            // If this method is called from a timer, prevent the blocking calls from beeing queued up if there are no more frames available.
            if (this.FrameDelay > 0)
            {
                Frame nextFrame;
                var frameAvailable = this.readyFrames.TryTake(out nextFrame);
                if (frameAvailable)
                {
                    this.FireFrameReady(nextFrame);
                }
            }
            // Otherwise the method is called from a loop. We should block and wait for the next frame.
            else
            {
                Frame nextFrame = this.readyFrames.Take(this._cancellationTokenSource.Token);
                this.FireFrameReady(nextFrame);
            }
        }

        private void ConsumeFrameBuffer()
        {
            while (!this._cancellationTokenSource.Token.IsCancellationRequested)
            {
                this.ConsumeFrame();
            }
        }

        private void FillFrameBuffer()
        {
            // load file
            var sizeBytes = new byte[NextFrameSizeLength];
            var serializer = new LeapFrameSerializer();
            try
            {
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

                            this.readyFrames.Add(frame, this._cancellationTokenSource.Token);
                        }
                        else if (this.LoopOutput)
                        {
                            // If the output should loop, reset the stream when the end of the file is reached.
                            file.Seek(0, SeekOrigin.Begin);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                
            }
            finally
            {
                this.fileClosedAutoResetEvent.Set();
            }
        }

        private void FireFrameReady(Frame f)
        {
            this.FrameReady?.Invoke(f);
        }
    }
}