using System.Threading;
using GestureRecognition.Implementation;
using GestureRecognition.Interface;
using GestureRecognition.Interface.Commands;
using NUnit.Framework;

namespace GestureRecognitionTest
{
    [TestFixture]
    public class LeapGestureControllerTest
    {

        [Test]
        [Ignore("This tests needs a connected Leap Motion. It will be ignored in continous integration")]
        public void TestLeapInit()
        {
            var commandReceived = false;
            var commandWaiter = new ManualResetEventSlim();
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController());
            gestureRecognition.SubscribeToCommand<PhysicCommand>(x =>
            {
                commandReceived = true;
                commandWaiter.Set();
            });

            commandWaiter.Wait(5000);

            Assert.IsTrue(commandReceived);
        }
    }
}