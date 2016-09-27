namespace GestureRecognitionTest.Implementation
{
    using System;
    using System.Threading;

    using GestureRecognition.Implementation;

    using Leap;

    using NUnit.Framework;

    using Timer = System.Timers.Timer;

    [TestFixture]
    public class LeapPlayerTest
    {
        /// <summary>
        /// Tests if the LeapPlayers data is looped when the end of the underlying file is reached.
        /// </summary>
        [Test]
        public void TestLeapPlayerLoop()
        {
            for (int i = 0; i < 3; i++)
            {
                var testOver = new ManualResetEventSlim();
                var timeoutTimer = new Timer(10000);

                var player = new LeapPlayer(@"frames\rotate_1.frames");
                player.LoopOutput = true;

                var bytesExpected = player.FileSizeInBytes * 3;

                Action<Frame> frameListener = frame =>
                    {
                        timeoutTimer.Stop();
                        timeoutTimer.Start();
                        if (player.TotalBytesRead >= bytesExpected)
                        {
                            testOver.Set();
                        }
                    };
                player.FrameReady += frameListener;

                timeoutTimer.Elapsed += (sender, args) => testOver.Set();
                player.StartConnection();

                timeoutTimer.Start();
                
                testOver.Wait();

                player.StopConnection();

                Assert.IsTrue(player.TotalBytesRead >= bytesExpected);
            }
        }

        /// <summary>
        /// Tests if the LeapPlayers data is looped when the end of the underlying file is reached.
        /// This test uses a delay of 1 millisecond between frames.
        /// FIXME: This method never terminates
        /// </summary>
        /// [Test]
        public void TestLeapPlayerLoopWithDelay()
        {
            for (int i = 0; i < 3; i++)
            {
                var testOver = new ManualResetEventSlim();
                var timeoutTimer = new Timer(10000);

                var player = new LeapPlayer(@"frames\rotate_1.frames");
                player.LoopOutput = true;
                player.FrameDelay = 1;

                var bytesExpected = player.FileSizeInBytes * 3;

                Action<Frame> frameListener = frame =>
                    {
                        timeoutTimer.Stop();
                        timeoutTimer.Start();
                        if (player.TotalBytesRead >= bytesExpected)
                        {
                            testOver.Set();
                        }
                    };
                player.FrameReady += frameListener;

                timeoutTimer.Elapsed += (sender, args) => testOver.Set();
                player.StartConnection();

                timeoutTimer.Start();

                testOver.Wait();

                player.StopConnection();

                Assert.IsTrue(player.TotalBytesRead >= bytesExpected);
            }
        }
    }
}