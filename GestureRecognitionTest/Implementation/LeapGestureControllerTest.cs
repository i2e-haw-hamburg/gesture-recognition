using System;
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
            var commandFired = new ManualResetEvent(false);
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController());
            gestureRecognition.SubscribeToCommand<PhysicCommand>(x => commandFired.Set());
            Assert.IsTrue(commandFired.WaitOne(5000));
        }

        [Test]
        public void TestThumbsHasNoIntermediate()
        {
            var hasMetacarpal = true;
            var commandFired = new ManualResetEvent(false);
            var player = new LeapPlayer(@"frames\right_hand_grab.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<PhysicCommand>(x =>
            {
                hasMetacarpal = x.BodyParts.Any(part => part.Id == 20611 || part.Id == 22611);
                commandFired.Set();
            });
            player.StartConnection();
            Assert.IsTrue(commandFired.WaitOne(2000), "Physics command should be fired");
            player.StopConnection();
            Assert.IsFalse(hasMetacarpal, "Thumbs should not have an metacarpal bone");
        }
        
        [Test]
        public void TestRightHandGrab()
        {
            var rightHand = false;
            var grabDetected = new ManualResetEvent(false);
            var player = new LeapPlayer(@"frames\right_hand_grab.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<GrabCommand>(x =>
            {
                rightHand = x.RightHand;
                grabDetected.Set();
            });
            player.StartConnection();
            Assert.IsTrue(grabDetected.WaitOne(2000), "Grab Command of right hand should be detected");
            player.StopConnection();
            Assert.IsTrue(rightHand, "Right hand should be provided by command");
        }

        [Test]
        public void TestLeftHandGrab()
        {
            var grabDetected = new ManualResetEvent(false);
            var leftHand = false;
            var player = new LeapPlayer(@"frames\left_hand_grab.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<GrabCommand>(x =>
            {
                leftHand = x.LeftHand;
                grabDetected.Set();
            });
            player.StartConnection();
            Assert.IsTrue(grabDetected.WaitOne(6000), "Grab Command of left hand should be detected");
            player.StopConnection();
            Assert.IsTrue(leftHand, "Left hand should be provided by command");
        }

        [Test]
        public void TestRotate()
        {
            var rotated = false;
            var rotateDetected = new ManualResetEvent(false);
            var player = new LeapPlayer(@"frames\rotate_1.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<ScaleAndRotate>(x =>
            {
                rotated = Math.Abs(x.Rotation.X) > 0.001;
                rotateDetected.Set();
            });
            player.StartConnection();
            Assert.IsTrue(rotateDetected.WaitOne(2000), "Scale And Rotate Command should be detected");
            player.StopConnection();
            Assert.IsTrue(rotated, "Rotation should be true.");
        }
        
        public void ScaleUpTest()
        {
            var scale = false;
            var scaleDetected = new ManualResetEvent(false);
            var player = new LeapPlayer(@"frames\scale_up.frames");
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController(player));
            gestureRecognition.SubscribeToCommand<ScaleAndRotate>(x =>
            {
                scale = Math.Abs(x.Scale - 1.0) > 0.001;
                scaleDetected.Set();
            });
            player.StartConnection();
            Assert.IsTrue(scaleDetected.WaitOne(2000), "Scale And Rotate Command should be detected");
            player.StopConnection();
            Assert.IsTrue(scale, "Scale should be true.");
        }
    }
}