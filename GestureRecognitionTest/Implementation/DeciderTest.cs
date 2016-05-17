using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using GestureRecognition.Implementation;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using NUnit.Framework;
using Trame;

namespace GestureRecognitionTest.Implementation
{
    [TestFixture]
    public class DeciderTest
    {
        private IDecider decider;

        [SetUp]
        public void SetUp()
        {
            decider = new Decider();
        }

        [Test]
        public void TestDecideWithGoodResult()
        {
            var commandReceived = false;
            var commandWaiter = new ManualResetEventSlim();
            decider.NewInterpretedCommand += command =>
            {
                commandReceived = true;
                commandWaiter.Set();
            };
            decider.Decide(new List<Result> {MockResult(0.1), MockResult(0.04), MockResult(0.7)});

            commandWaiter.Wait(1000);
            Assert.IsTrue(commandReceived);
        }

        [Test]
        public void TestDecideWithBadResults()
        {
            var commandReceived = false;
            var commandWaiter = new ManualResetEventSlim();
            decider.NewInterpretedCommand += command =>
            {
                commandReceived = true;
                commandWaiter.Set();
            };
            decider.Decide(new List<Result> { MockResult(0.1), MockResult(0.04), MockResult(0.5) });

            commandWaiter.Wait(1000);
            Assert.IsFalse(commandReceived);
        }

        private Result MockResult(double prob)
        {
            return new Result(new ExplodeGesture(), prob, new Dictionary<JointType, InputVector>());
        }
    }
}