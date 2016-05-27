using System.Linq;
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

        [Test]
        [Ignore("This tests needs a connected Leap Motion. It will be ignored in continous integration")]
        public void TestThumbsHasNoIntermediate()
        {
            var hasIntermediate = false;
            var commandWaiter = new ManualResetEventSlim();
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController());
            gestureRecognition.SubscribeToCommand<PhysicCommand>(x =>
            {
                hasIntermediate = x.BodyParts.Any(part => part.Id == 20613 || part.Id == 22613);
                commandWaiter.Set();
            });
            commandWaiter.Wait(5000);
            Assert.IsFalse(hasIntermediate, "Thumbs should not have an intermediate bone");
        }
    }
}