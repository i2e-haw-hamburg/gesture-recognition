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
        public void TestThumbsHasNoIntermediate()
        {
            var hasMetacarpal = true;
            var commandFired = false;
            var commandWaiter = new ManualResetEventSlim();
            var player = new LeapPlayer(@"frames\right_hand_grab.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<PhysicCommand>(x =>
            {
                hasMetacarpal = x.BodyParts.Any(part => part.Id == 20611 || part.Id == 22611);
                commandFired = true;
                commandWaiter.Set();
            });
            player.StartConnection();
            commandWaiter.Wait(3000);
            player.StopConnection();
            Assert.IsTrue(commandFired, "Physics command should be fired");
            Assert.IsFalse(hasMetacarpal, "Thumbs should not have an metacarpal bone");
        }
        
        [Test]
        public void TestRightHandGrab()
        {
            var grabDetected = false;
            var commandWaiter = new ManualResetEventSlim();
            var player = new LeapPlayer(@"frames\right_hand_grab.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<GrabCommand>(x =>
            {
                grabDetected = true;
                commandWaiter.Set();
            });
            player.StartConnection();
            commandWaiter.Wait(3000);
            player.StopConnection();
            Assert.IsTrue(grabDetected, "Grab Command of right hand should be detected");
        }

        [Test]
        public void TestLeftHandGrab()
        {
            var grabDetected = false;
            var commandWaiter = new ManualResetEventSlim();
            var player = new LeapPlayer(@"frames\left_hand_grab.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<GrabCommand>(x =>
            {

                grabDetected = true;
                commandWaiter.Set();
            });
            player.StartConnection();
            commandWaiter.Wait(3000);
            player.StopConnection();
            Assert.IsTrue(grabDetected, "Grab Command of left hand should be detected");
        }
    }
}